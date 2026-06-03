async function RemoveAdmin(userIds) {
    const response = await fetch(`/api/user/RemoveAdmin`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(userIds)
    });
}

async function MakeAdmin(userIds) {
    const response = await fetch(`/api/user/MakeAdmin`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(userIds)
    });
}

async function BlockUsers(userIds) {
    const response = await fetch(`/api/user/BlockUsers`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(userIds)
    });
}

async function UnblockUsers(userIds) {
    const response = await fetch(`/api/user/UnblockUsers`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(userIds)
    });
}

async function DeleteUsers(usersId) {
    const response = await fetch('/api/user/DeleteUsers', {
        method: 'DELETE',
        headers: {
            'Content-type': 'application/json'
        },
        body: JSON.stringify(usersId)
    });
}

async function GetAllUsers() {
    const response = await fetch('/api/user/GetAllUsers');
    return response;
}

async function MakeEditor(userIds, inventoryId) {
    await fetch(`/api/user/MakeEditor?idInventory=${inventoryId}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(userIds)
    });
}

async function RemoveEditor(tableIds, inventoryId) {
    await fetch(`/api/user/RemoveEditor?idInventory=${inventoryId}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(tableIds)
    });
}

async function GetEditorInventory(inventoryId, userName, searchField) {
    const response = await fetch(`/api/user/GetEditorInventory?idInventory=${inventoryId}&userName=${userName}&searchField=${searchField}`);

    return response;
}

async function UserByEmail(email) {
    const response = await fetch(`/api/user/UserByEmail?email=${email}`);

    return response;
}

async function UserByEmail(email) {
    const response = await fetch(`/api/user/UserByEmail?email=${email}`);

    return response;
}

async function UpdateContact(id, lastName, description) {
    const response = await fetch(`/api/user/UpdateContact?id=${id}&lastName=${lastName}&description=${description}`, {
        method: 'PATCH',
        headers: {
            'Content-Type': 'application/json',
        }
    });

    return response;
}