



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