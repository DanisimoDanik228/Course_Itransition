async function RemoveAdmin(userIds) {
    const response = await fetch(`/User/RemoveAdmin`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(userIds)
    });
}

async function MakeAdmin(userIds) {
    const response = await fetch(`/User/MakeAdmin`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(userIds)
    });
}

async function BlockUsers(userIds) {
    const response = await fetch(`/User/BlockUsers`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(userIds)
    });
}

async function UnblockUsers(userIds) {
    const response = await fetch(`/User/UnblockUsers`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(userIds)
    });
}

async function DeleteUsers(usersId) {
    const response = await fetch('/User/DeleteUsers', {
        method: 'DELETE',
        headers: {
            'Content-type': 'application/json'
        },
        body: JSON.stringify(usersId)
    });
}

async function DeleteInventory(inventoryIds) {
    await fetch('/Home/DeleteInventory', {
        method: 'DELETE',
        headers: {
            'Content-type': 'application/json'
        },
        body: JSON.stringify(inventoryIds)
    });
}

async function AddInventory(inventory) {
    const response = await fetch('/Home/AddInventory', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(inventory)
    });

    return response;
}

async function GetAllPartCustomId() {
    const response = await fetch('/Home/GetAllPartCustomId');

    return response;
}

async function GetAllInventory() {
    const response = await fetch('/Home/GetAllInventory');

    return response;
}

async function GetAllUserAccessInventories(userId) {
    const response = await fetch(`/Home/GetAllUserAccessInventories?userId=${userId}`);

    return response;
}

async function GetAllUserInventories(userId) {
    const response = await fetch(`/Home/GetAllUserInventories?userId=${userId}`);

    return response;
}

async function GetAllUserAccessInventories(userId) {
    const response = await fetch(`/Home/GetAllUserAccessInventories?userId=${userId}`);

    return response;
}

async function GetAllUsers(){
    const response = await fetch('/User/GetAllUsers');
    return response;
}

async function GetPartInventory(id,page) {
    const response = await fetch(`/Home/GetPartInventory?idInventory=${id}&Page=${page}`);   
    return response;
}

async function AddItemOnInventory(id, items) {
    const response = await fetch(`/Home/AddItem?idInventory=${id}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(items)
    });

    return response;
}

async function AddItemValueOnInventory(id, items) {
    const response = await fetch(`/Home/AddItemValue?idInventory=${id}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(items)
    });

    return response;
}

async function AddFieldOnInventory(id,fieldName,fieldType){
    const item = {
        id: 0,
        inventoryId: id,
        name: fieldName,
        type: fieldType
    };

    const response = await fetch(`/Home/AddField`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(item)
    });

    return response;
}

async function DeleteItemsOnInventory(id, itemsId){
    await fetch(`/Home/DeleteItems?idInventory=${id}`, {
        method: 'DELETE',
        headers: {
            'Content-type': 'application/json'
        },
        body: JSON.stringify(itemsId)
    });
}

async function DeleteFieldOnInventory(id,fieldIds){
    const response = await fetch(`/Home/DeleteField?idInventory=${id}`, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(fieldIds)
    });

    return response;
}

async function MakeEditor(userIds, inventoryId) {
    await fetch(`/User/MakeEditor?idInventory=${inventoryId}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(userIds)
    });
}

async function RemoveEditor(tableIds, inventoryId) {
    await fetch(`/User/RemoveEditor?idInventory=${inventoryId}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(tableIds)
    });
}

async function UpdateItem(inventoryId, items) {
    await fetch(`/Home/UpdateItem?idInventory=${inventoryId}`, {
        method: 'PATCH',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(items)
    });
}

async function GetEditorInventory(inventoryId, userName, searchField) {
    const response = await fetch(`/User/GetEditorInventory?idInventory=${inventoryId}&userName=${userName}&searchField=${searchField}`);

    return response;
}

// funcToId : i => i.dataset.id
function getSelected(className, funcToId = i => i.dataset.id) {
    const res = document.getElementsByClassName(className);
    const iDs = Array.from(res)
        .filter(i => i.checked)
        .map(funcToId);

    return iDs;
}

function createClickSelectAll(idMainCheckBox, checkBoxClassName) {
    const checkBox = document.getElementById(idMainCheckBox);

    checkBox.addEventListener('change', (event) => {
        const allCheckBoxes = document.getElementsByClassName(checkBoxClassName);

        for (let item of allCheckBoxes) {
            item.checked = event.target.checked;
        }
    });
}