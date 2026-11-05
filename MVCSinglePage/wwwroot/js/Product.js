const productForm = document.getElementById('productForm');
//inputs
const productIdInput = document.getElementById('productId');
const titleInput = document.getElementById('title');
const unitPriceInput = document.getElementById('unitPrice');
const descriptionRecordInput = document.getElementById('descriptionRecord');
//Message
const resultMessage = document.getElementById('resultMessage');
const formModeText = document.getElementById('formModeText');
//detail input
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
//Others
const productsTableBody = document.getElementById('productsTableBody');
const totalCount = document.getElementById('totalCount');
//variable
let products=[];
let selectedProductId=null;
let isEditing=false;
//Events


LoadProducts();
async function LoadProducts(){
    try{
    const response=await fetch("/Product/GetAll");
    const product=await response.json();
    ShowProducts();
    EnableButtons();


    }catch(error){

        alert("Operation failed");
    }
}
function ShowProducts(){
    totalCount.value=products.length;
    productsTableBody.innerHTML="";

    if(products.length===0){
        productsTableBody.innerHTML=`
            <tr>
                <td colspan="4">No Person has been added yet</td>
            </tr>
        `;
        return;
    }
    products.forEach(item=>{
        const row=document.createElement("tr");
        if(item.id===selectedProductId){

        }
        row.innerHTML = `
    <td>
        <input type="checkbox"
            class="product-select"
            ${item.id === selectedProductId ? 'checked' : ''}>
    </td>
    <td>${item.title}</td>
    <td>${item.unitPrice}</td>
    <td>${item.descriptionRecord}</td>
    `;
    row.setAttribute('product-id',item.id);
    productsTableBody.appendChild(row);
    });
}
