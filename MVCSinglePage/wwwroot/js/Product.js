//form
const productForm = document.getElementById('productForm');
//inputs
const productIdInput = document.getElementById('productId');
const titleInput = document.getElementById('title');
const unitPriceInput = document.getElementById('unitPrice');
const descriptionRecordInput = document.getElementById('descriptionRecord');
//
const resultMessage = document.getElementById('resultMessage');
const formModeText = document.getElementById('formModeText');
//details
const detailPanel = document.getElementById('detailPanel');
const detailTitle = document.getElementById('detailTitle');
const detailUnitPrice = document.getElementById('detailUnitPrice');
const detailDescription = document.getElementById('detailDescription');
const detailId = document.getElementById('detailId');
//buttons
const btnAdd = document.getElementById('btnAdd');
const btnSave = document.getElementById('btnSave');
const btnEdit = document.getElementById('btnEdit');
const btnDetail = document.getElementById('btnDetail');
const btnDelete = document.getElementById('btnDelete');
const btnCancelEdit = document.getElementById('btnCancelEdit');
//others
const productsTableBody = document.getElementById('productsTableBody');
const totalCount = document.getElementById('totalCount');
//variables
let products = [];
let selectedProductId = null;
let isEditing = false;

productForm.addEventListener('submit', saveProduct);
productsTableBody.addEventListener('change', selectFromCheckbox);
btnAdd.addEventListener('click', resetForm);
btnCancelEdit.addEventListener('click', resetForm);
btnEdit.addEventListener('click', editProduct);
btnDetail.addEventListener('click', showDetails);
btnDelete.addEventListener('click', deleteProduct);

loadProducts();

async function loadProducts() {
    try {
        const response = await fetch('/Product/GetAll');
        if (!response.ok) throw new Error('Could not load products');
        products = await response.json();
        showProducts();
        enableButtons();
    } catch (error) {
        showMessage('Could not load products.');
    }
}

function showProducts() {
    totalCount.textContent = products.length;
    productsTableBody.replaceChildren();

    if (products.length === 0) {
        const row = document.createElement('tr');
        const cell = document.createElement('td');
        cell.colSpan = 4;
        cell.textContent = 'No products have been added yet';
        row.appendChild(cell);
        productsTableBody.appendChild(row);
        return;
    }

    products.forEach(product => {
        const row = document.createElement('tr');
        row.dataset.productId = product.id;

        const checkboxCell = document.createElement('td');
        const checkbox = document.createElement('input');
        checkbox.type = 'checkbox';
        checkbox.className = 'product-select';
        checkbox.checked = product.id === selectedProductId;
        checkboxCell.appendChild(checkbox);
        row.appendChild(checkboxCell);

        [product.title, product.unitPrice, product.descriptionRecord].forEach(value => {
            const cell = document.createElement('td');
            cell.textContent = value ?? '';
            row.appendChild(cell);
        });
        productsTableBody.appendChild(row);
    });
}

function selectFromCheckbox(event) {
    const checkbox = event.target;
    if (!checkbox.matches('.product-select')) return;

    selectedProductId = checkbox.checked
        ? checkbox.closest('tr').dataset.productId
        : null;
    detailPanel.hidden = true;
    showProducts();
    enableButtons();
}

function enableButtons() {
    const hasSelection = selectedProductId !== null;
    btnEdit.disabled = !hasSelection;
    btnDetail.disabled = !hasSelection;
    btnDelete.disabled = !hasSelection;
}

function editProduct() {
    const product = products.find(item => item.id === selectedProductId);
    if (!product) return;

    isEditing = true;
    productIdInput.value = product.id;
    titleInput.value = product.title;
    unitPriceInput.value = product.unitPrice;
    descriptionRecordInput.value = product.descriptionRecord ?? '';
    formModeText.textContent = 'Edit product';
    btnSave.textContent = 'Save changes';
    btnCancelEdit.hidden = false;
    detailPanel.hidden = true;
}

async function saveProduct(event) {
    event.preventDefault();
    if (!productForm.reportValidity()) return;

    const product = {
        // The API binds this value to a non-nullable .NET Guid.
        id: productIdInput.value || '00000000-0000-0000-0000-000000000000',
        title: titleInput.value.trim(),
        unitPrice: Number(unitPriceInput.value),
        descriptionRecord: descriptionRecordInput.value.trim()
    };

    try {
        const response = await fetch(isEditing ? '/Product/Edit' : '/Product/Create', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(product)
        });
        const responseBody = await response.text();
        const result = responseBody ? JSON.parse(responseBody) : {};
        if (!response.ok || !result.success) {
            showMessage(result.message || 'Operation failed.');
            return;
        }

        selectedProductId = null;
        resetForm();
        await loadProducts();
        showMessage('Product saved successfully.');
    } catch (error) {
        showMessage('Product could not be saved.');
    }
}

async function showDetails() {
    if (!selectedProductId) return;
    try {
        const response = await fetch(`/Product/Detail?id=${encodeURIComponent(selectedProductId)}`);
        if (!response.ok) throw new Error('Could not load product details');
        const product = await response.json();
        detailTitle.textContent = product.title ?? '';
        detailUnitPrice.textContent = product.unitPrice ?? '';
        detailDescription.textContent = product.descriptionRecord ?? '';
        detailId.textContent = product.id ?? '';
        detailPanel.hidden = false;
    } catch (error) {
        showMessage('Could not load product details.');
    }
}

async function deleteProduct() {
    if (!selectedProductId || !confirm('Are you sure you want to delete this product?')) return;
    try {
        const response = await fetch('/Product/Delete', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ id: selectedProductId })
        });
        const responseBody = await response.text();
        const result = responseBody ? JSON.parse(responseBody) : {};
        if (!response.ok || !result.success) {
            showMessage(result.message || 'Operation failed.');
            return;
        }

        selectedProductId = null;
        resetForm();
        await loadProducts();
        showMessage('Product deleted successfully.');
    } catch (error) {
        showMessage(error.message || 'Product could not be deleted.');
    }
}

function resetForm() {
    productForm.reset();
    productIdInput.value = '';
    isEditing = false;
    formModeText.textContent = 'Add a new product or select a row to edit.';
    btnSave.textContent = 'Save product';
    btnCancelEdit.hidden = true;
    detailPanel.hidden = true;
}

function showMessage(message) {
    resultMessage.textContent = message;
}
