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

function collapseFieldOrder(currentIds, allIds) {
    let newOrder = [];

    for (let id of currentIds)
        if (allIds.includes(id))
            newOrder.push(id);

    for (let id of allIds)
        if (!currentIds.includes(id))
            newOrder.push(id);

    return newOrder;
}

function changeTheme() {
    const htmlEl = document.documentElement;
    const currentTheme = htmlEl.getAttribute('data-bs-theme');
    const newTheme = currentTheme === 'dark' ? 'light' : 'dark';
    htmlEl.setAttribute('data-bs-theme', newTheme);
    localStorage.setItem('theme', newTheme)
}