const API_URL = '/Person';

//form  
const form =document.getElementById("personForm");
//allow buttons
const btnSavePerson = document.getElementById("btnSavePerson");
const btnDeleteConfirm = document.getElementById("btnDeleteConfirm");
//button  modal
const btnClose = document.getElementsByClassName("btn-close btn-close-white");
const btnCancel = document.getElementsByClassName("btn btn-secondary");
//check box
const chkSelectAll = document.getElementById("selectAll");
//input
const inDeleteConfirmModalId = document.getElementById("inDeleteConfirmModalId");
const personId = document.getElementById("personId");
const firstName = document.getElementById("firstName");
const lastName = document.getElementById("lastName");
const age = document.getElementById("age");
//btn Open modals
const openDetailsModal = document.getElementById("btnOpenDetailsModal");
const openAddModal = document.getElementById("btnOpenAddModal");
const openDeleteModal = document.getElementById("btnOpenDeleteModal");
const openEditModal = document.getElementById("btnOpenEditModal");
//ValidationMessages
const firstNameValidationMessage = document.getElementById("firstNameValidationMessage");
const lastNameValidationMessage = document.getElementById("lastNameValidationMessage");
const ageValidationMessage = document.getElementById("ageValidationMessage");
//others
const resultMessage = document.getElementById("resultMessage");

//======Event listener======
btnSavePerson.addEventListener("click",Add);
openEditModal.addEventListener("click",Edit)
form.addEventListener("submit", Add);

//=====functions============
//خالی کردن فرم 
function RefreshForm(){
    firstName.classList.remove("is-invalid", "is-valid");
    lastName.classList.remove("is-invalid", "is-valid");
    age.classList.remove("is-invalid", "is-valid");

    firstName.value = "";
    lastName.value = "";
    age.value = "";

    firstNameValidationMessage.innerText = "";
    lastNameValidationMessage.innerText = "";
    ageValidationMessage.innerText = "";


}
//اعتبارسنجی اطلاعات در فرم 
function ValidateFormData() {
    var isValidData = true;
    if (firstName.value == "") {
        firstNameValidationMessage.innerText = "firstname is required";
        firstNameValidationMessage.classList.add("is-invalid");
        isValidData = false;
    }
    else {
        firstNameValidationMessage.classList.add("is-valid");
    }

    if (lastName.value == "") {
        lastNameValidationMessage.innerText = "lastname is required";
        lastNameValidationMessage.classList.add("is-invalid");
        isValidData = false;
    }
    else {
        lastNameValidationMessage.classList.add("is-valid");
    }

    if (age.value == ""||age.value<=0) {
        ageValidationMessage.innerText = "age  is required";
        ageValidationMessage.classList.add("is-invalid");
        isValidData = false;
    }
    else {
        ageValidationMessage.classList.add("is-valid");
    }
    return isValidData;
}
//نمایش دیتا در جدول
window.addEventListener("DOMContentLoaded", LoadData);
async function LoadData() {
    console.log("page is loading...");
    RefreshForm();
    chkSelectAll.checked = false;
    DeselectAll();
    try {
        const response = await fetch("/Person/GetAll");
        if (!response.ok) {
            throw new Error("Network response was not ok");
        }
        const dto = await response.json();
        console.log({ dto });
        console.table(dto);
        const html = dto.map((item) =>
            `<tr id="${item.id}">
            <td><input type="checkbox"></td>
            <td id="${item.id}"></td>
            <td><input id="selectRow" class="form-check-input" type="checkbox" name="${item.id}" onClick="SelectRow(this);"/></td>
            <td id="fntd" class="fn" >${item.firstName}</td>
            <td id="lntd" class="ln" >${item.lastName}</td>
            <td id="agtd" class="age" >${item.age}</td>
            <td>
            <input class="btn btn-outline-primary btn-sm" type="button" value="Details" data-bs-toggle="modal" data-bs-target="#detailsModal">
            <input class="btn btn-outline-danger btn-sm" type="button" value="Delete" data-bs-toggle="modal" data-bs-target="#deleteConfirmModal">  
            </td>
            </tr>`
        ).join("");
        document.querySelector("#personsTable tbody").innerHTML = html;

    }
    catch (error) {
        console.error("Error loading data:", error);
    }
}
async function Add(e) {
    e.preventDefault();
    if (!ValidateFormData()) return;

    const dto = {
        id: "",
        firstName: firstName.value.trim(),
        lastName: lastName.value.trim(),
        age: age.value.trim(),
    };
    try {
        const response = await fetch("/Person/Create", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                Accept: "*/*",
            },
            body: JSON.stringify(dto),
        });
        if (response.ok) {
            TriggerResultMessage("Operation successful");
            await LoadData();
            const modalElement = document.getElementById("personModal");
            const modalInstance = bootstrap.Modal.getInstance(modalElement);
            if (modalInstance) {
                modalInstance.hide();
            }
        
        } else {
            TriggerResultMessage("Operation Failed");

        }
    }
    catch (error){
        console.error("error adding:", error);
        TriggerResultMessage("Operation Failed");

    }
}

//ویرایش دیتا
async function Edit() {
    
    const dto = {
        id: personId.value,
        firstName: firstName.value.trim(),
        lastName: lastName.value.trim(),
        age: age.value.trim(),
        };
    try {
        const response = await fetch("/Person/Edit", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                Accept: "*/*",
            },
            body: JSON.stringify(dto),
        });
        if (response.ok) {
            TriggerResultMessage("Operation successful");
            await LoadData();
        } else {
            TriggerResultMessage("Operation failed");
        }
    }

    catch (error) {
        console.error("Error updating person:", error);
        TriggerResultMessage("Operation failed");

    }
}

//نمایش دیتا
// async function Details() {
//     LoadData()
// }


//انتخاب یک  سطر
selectedRowsList = [];
allRowsCount=0;
async function SelectRow(checkbox) {
    if (checkbox.checked) {
        selectedRowsList.push(checkbox.name)
    } else {
        selectedRowsList = selectedRowsList.filter(id => id !== checkbox.name);
    }
    chkSelectAll.checked = (selectedRowsList.length === allRowsCount && allRowsCount > 0);
    if (selectedRowsList.length === 1) {
        openEditModal.disabled = false;
        RefreshForm();
        const row = document.getElementById(selectedRowsList[0]);
        personId.value = checkbox.value;
        firstName.value = document.querySelector(".fn").innerText.trim();
        lastName.value = document.querySelector(".ln").innerText.trim();
        age.value = document.querySelector(".age").innerText.trim();
    } else {
        openEditModal.disabled = true;
        RefreshForm();
    }
}


 // مدیریت فعال/غیرفعال کردن دکمه‌ها بر اساس انتخاب‌ها
function ToggleActionButtons() {
    if (selectedRowsList.length >= 1) {
        openDeleteModal.disabled = false;
    } else {
        openDeleteModal.disabled = true;
    }
    if (selectedRowsList.length === 1) {
        openEditModal.disabled = false;
    } else {
        openEditModal.disabled = true;
    }
}

// مدیریت انتخاب/عدم انتخاب همه چک‌باکس‌ها
function SelectAll() {

}

//پیام نتیجه
function TriggerResultMessage(message) {
    resultMessage.innerText = message;
    resultMessage.style.opacity = "1";
    setTimeout(() => {
        resultMessage.style.opacity = "0";
    }, 2000);
}