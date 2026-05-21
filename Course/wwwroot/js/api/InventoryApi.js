async function DeleteInventory(inventoryIds) {
    await fetch('/api/inventory/DeleteInventory', {
        method: 'DELETE',
        headers: {
            'Content-type': 'application/json'
        },
        body: JSON.stringify(inventoryIds)
    });
}

async function AddInventory(inventory) {
    const response = await fetch('/api/inventory/AddInventory', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(inventory)
    });

    return response;
}

async function SetStructCustomId(inventoryId, structCustomId) {
    const response = await fetch(`/api/inventory/SetStructCustomId?idInventory=${inventoryId}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(structCustomId)
    });

    return response;
}

async function GetStructCustomId(inventoryId) {
    const response = await fetch(`/api/inventory/GetStructCustomId?idInventory=${inventoryId}`);

    return response;
}

async function GetAllPartCustomId() {
    const response = await fetch('/api/inventory/GetAllPartCustomId');

    return response;
}

async function GetAllInventory() {
    const response = await fetch('/api/inventory/GetAllInventory');

    return response;
}

async function GetAllUserAccessInventories(userId) {
    const response = await fetch(`/api/inventory/GetAllUserAccessInventories?userId=${userId}`);

    return response;
}

async function GetAllUserInventories(userId) {
    const response = await fetch(`/api/inventory/GetAllUserInventories?userId=${userId}`);

    return response;
}

async function GetInventoryTypes(inventoryId) {
    const response = await fetch(`/api/inventory/GetInventoryTypes?idInventory=${inventoryId}`);

    return response;
}

async function GetAllUserAccessInventories(userId) {
    const response = await fetch(`/api/inventory/GetAllUserAccessInventories?userId=${userId}`);

    return response;
}

async function GetPartInventory(id, page) {
    const response = await fetch(`/api/inventory/GetPartInventory?idInventory=${id}&Page=${page}`);
    return response;
}

async function GetOrderField(idInventory) {
    const response = await fetch(`/api/inventory/GetOrderField?idInventory=${idInventory}`);
    return response;
}

async function GetStatusInventory(idInventory) {
    const response = await fetch(`/api/inventory/GetStatusInventory?idInventory=${idInventory}`);
    return response;
}

async function AddItemOnInventory(id, items) {
    const response = await fetch(`/api/inventory/AddItem?idInventory=${id}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(items)
    });

    return response;
}

async function AddItemValueOnInventory(id, items) {
    const response = await fetch(`/api/inventory/AddItemValue?idInventory=${id}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(items)
    });

    return response;
}

async function AddFieldOnInventory(item) {
    const response = await fetch(`/api/inventory/AddField`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(item)
    });

    return response;
}

async function DeleteItemsOnInventory(id, itemsId) {
    await fetch(`/api/inventory/DeleteItems?idInventory=${id}`, {
        method: 'DELETE',
        headers: {
            'Content-type': 'application/json'
        },
        body: JSON.stringify(itemsId)
    });
}

async function DeleteFieldOnInventory(id, fieldIds) {
    const response = await fetch(`/api/inventory/DeleteField?idInventory=${id}`, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(fieldIds)
    });

    return response;
}

async function UpdateItem(inventoryId, items) {
    await fetch(`/api/inventory/UpdateItem?idInventory=${inventoryId}`, {
        method: 'PATCH',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(items)
    });
}

async function UpdateInvertoryTypes(items) {
    await fetch(`/api/inventory/UpdateInvertoryTypes`, {
        method: 'PATCH',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(items)
    });
}
async function UpdateOrderField(inventoryId, orderField) {
    await fetch(`/api/inventory/UpdateOrderField?inventoryId=${inventoryId}`, {
        method: 'PATCH',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(orderField)
    });
}

async function UpdateStatusInventory(idInventory, status) {
    await fetch(`/api/inventory/UpdateStatusInventory?idInventory=${idInventory}&status=${status}`, {
        method: 'PATCH',
        headers: {
            'Content-Type': 'application/json',
        }
    });
}