async function RemoveAdmin(userIds) {
    const request = {
        ids: userIds
    };

    const response = await fetch(`/User/RemoveAdmin`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(request)
    });
}

async function MakeAdmin(userIds) {
    const request = {
        ids: userIds
    };

    const response = await fetch(`/User/MakeAdmin`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(request)
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

async function GetAllUsers(){
    const response = await fetch('/User/GetAllUsers');
    return await response.json();
}

function getSelected(className) {
    const res = document.getElementsByClassName(className);
    const iDs = Array.from(res)
        .filter(i => i.checked)
        .map(i => i.dataset.id);

    return iDs;
}