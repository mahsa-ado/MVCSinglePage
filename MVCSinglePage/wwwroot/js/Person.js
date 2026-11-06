
const personForm = document.getElementById('personForm');
// inputs

const personIdInput = document.getElementById('personId');
const firstNameInput = document.getElementById('firstName');
const lastNameInput = document.getElementById('lastName');
const ageInput = document.getElementById('age');
//others
const peopleTableBody = document.getElementById('peopleTableBody');
const totalCount = document.getElementById('totalCount');
const resultMessage = document.getElementById('resultMessage');
const formModeText = document.getElementById('formModeText');
//detail
const detailPanel = document.getElementById('detailPanel');
const detailName = document.getElementById('detailName');
const detailAge = document.getElementById('detailAge');
const detailId = document.getElementById('detailId');
//buttons
const btnAdd = document.getElementById('btnAdd');
const btnSave = document.getElementById('btnSave');
const btnEdit = document.getElementById('btnEdit');
const btnDetail = document.getElementById('btnDetail');
const btnDelete = document.getElementById('btnDelete');
const btnCancelEdit = document.getElementById('btnCancelEdit');

let people = [];
let selectedPersonId = null;
let isEditing = false;

// Event
personForm.addEventListener('submit', savePerson);
peopleTableBody.addEventListener('change', selectFromCheckbox);
btnAdd.addEventListener('click', resetForm);
btnCancelEdit.addEventListener('click', resetForm);
btnEdit.addEventListener('click', editPerson);
btnDetail.addEventListener('click', showDetails);
btnDelete.addEventListener('click', deletePerson);

// بارگذاری فهرست افراد هنگام باز شدن صفحه
loadPeople();

// گرفتن فهرست افراد از سرور
async function loadPeople() {
    try {
        const response = await fetch('/Person/GetAll');
        if (!response.ok) throw new Error('Could not load people');
        people = await response.json();
        showPeople();
        enableButtons();
    } catch (error) {
        showMessage('Operation failed');
    }
}

// نمایش افراد در جدول

function showPeople() {
    totalCount.textContent = people.length;
    peopleTableBody.innerHTML = '';

    if (people.length === 0) {
        peopleTableBody.innerHTML = `
            <tr>
                <td colspan="4">No Person has been added yet</td>
            </tr>
        `;
        return;
    }

    people.forEach(person => {
        const row = document.createElement('tr');

        // if (person.id === selectedPersonId) {
        //     row.classList.add('selected-row');
        // }

        const checkboxCell = document.createElement('td');
        const checkbox = document.createElement('input');
        checkbox.type = 'checkbox';
        checkbox.className = 'person-select';
        checkbox.checked = person.id === selectedPersonId;
        checkboxCell.appendChild(checkbox);
        row.appendChild(checkboxCell);

        [person.firstName, person.lastName, person.age].forEach(value => {
            const cell = document.createElement('td');
            cell.textContent = value ?? '';
            row.appendChild(cell);
        });
        row.dataset.personId = person.id;

        peopleTableBody.appendChild(row);
    });
}


// انتخاب یک نفر با چک‌باکس



function selectFromCheckbox(event) {

    const checkbox = event.target;

    // بررسی اینکه عنصر کلیک‌شده چک‌باکس باشد
    if (checkbox.className !== 'person-select') {
        return;
    }

    // پیدا کردن ردیف مربوط به چک‌باکس
    const row = checkbox.closest('tr');
    if (!row) return;

    // بررسی وضعیت چک‌باکس
    if (checkbox.checked ) {

        selectedPersonId = row.dataset.personId;

    } else {

        selectedPersonId = null;

    }

    // به روزرسانی جدول و دکمه ها
    showPeople();
    enableButtons();
}


// فعال یا غیرفعال کردن دکمه‌ها
function enableButtons() {

        if (selectedPersonId === null) {

            btnEdit.disabled = true;
            btnDetail.disabled = true;
            btnDelete.disabled = true;

        } else {

            btnEdit.disabled = false;
            btnDetail.disabled = false;
            btnDelete.disabled = false;

        }

    

}

// پر کردن فرم با اطلاعات فرد انتخاب‌شده
function editPerson() {

    // پیدا کردن شخص انتخاب شده
    let person = null;

    for (let i = 0; i < people.length; i++) {

        if (people[i].id == selectedPersonId) {
            person = people[i];
            break;
        }

    }
    if (person == null) {
        return;
    }

    isEditing = true;

    // قرار دادن اطلاعات شخص داخل فرم
    personIdInput.value = person.id;
    firstNameInput.value = person.firstName;
    lastNameInput.value = person.lastName;
    ageInput.value = person.age;
    formModeText.textContent = 'Edit';
    btnSave.textContent = 'Save Changes';

    // نمایش دکمه لغو ویرایش
    btnCancelEdit.hidden = false;
}


// افزودن فرد جدید یا ویرایش فرد انتخاب‌شده

async function savePerson(event) {
    event.preventDefault();

    if (!personForm.reportValidity()) return;

    let url = '';

    if (isEditing) {
        url = '/Person/Edit';
    } else {
        url = '/Person/Create';
    }

    const person = {
        // Guid properties must receive a valid GUID string, including on create.
        id: personIdInput.value || '00000000-0000-0000-0000-000000000000',
        firstName: firstNameInput.value,
        lastName: lastNameInput.value,
        age: Number(ageInput.value)
    };

    try {
        const response = await fetch(url, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(person)
        });

        const result = await response.json();

        if (!response.ok || !result.success) {
            showMessage(result.message || 'Operation failed');
            return;
        }

        resetForm();
        selectedPersonId = null;

        await loadPeople();

        showMessage('Saved successfully');

    } catch (error) {
        showMessage('It was not saved successfully');
    }
}


// نمایش جزئیات
async function showDetails() {
    try {
        if (!selectedPersonId) return;
        const response = await fetch(`/Person/GetPersonDetails?id=${encodeURIComponent(selectedPersonId)}`);
        if (!response.ok) throw new Error('Could not load person details');
        const person = await response.json();
        detailName.textContent = person.firstName + ' ' + person.lastName;
        detailAge.textContent = person.age;
        detailId.textContent = person.id;
        detailPanel.hidden = false;
    } catch (error) {
        showMessage('دریافت جزئیات انجام نشد.');
    }
}

// حذف
async function deletePerson() {
    if (!selectedPersonId || !confirm("Are you sure you want to delete this person?")) return;

    try {
        const response = await fetch('/Person/Delete', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ id: selectedPersonId })
        });

        const responseBody = await response.text();
        const result = responseBody ? JSON.parse(responseBody) : {};
        if (!response.ok || !result.success) {
            showMessage(result.message || 'Operation failed');
            return;
        }

        resetForm();
        selectedPersonId = null;
        await loadPeople();
        showMessage('Person deleted successfully');
    } catch (error) {
        showMessage(error.message || 'Person could not be deleted.');
    }
}

// خالی کردن فرم
function resetForm() {
    personForm.reset();
    personIdInput.value = '';
    isEditing = false;
    formModeText.textContent = 'Add a new person or select a row to edit.';
    btnSave.textContent = 'Save person';
    btnCancelEdit.hidden = true;
    detailPanel.hidden = true;
}

// نمایش پیام کوتاه
function showMessage(message) {
    resultMessage.textContent = message;
}
