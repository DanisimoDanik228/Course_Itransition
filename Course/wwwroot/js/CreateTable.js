function createTable(
    data,
    idMainCheckBox,
    checkBoxClassName,
    actionDeleteUsers) {
    const mainContainer = document.createElement('div');
    const table = document.createElement('table');

    const thead = document.createElement('thead');
    const headRow = document.createElement('tr');
    const tbody = document.createElement('tbody');

    const th = document.createElement('th');
    const checkBox = document.createElement('input');
    checkBox.type = 'checkbox';
    checkBox.id = idMainCheckBox;
    th.appendChild(checkBox);
    headRow.appendChild(th);

    for (let item of data.head) {
        const th = document.createElement('th');
        th.textContent = item;
        headRow.appendChild(th);
    }
    thead.appendChild(headRow);

    let allCheckBoxes = [];

    for (let itemArray of data.body) {
        const tr = document.createElement('tr');

        const td = document.createElement('td');
        const checkBox = document.createElement('input');
        checkBox.type = 'checkbox';
        checkBox.className = checkBoxClassName;
        checkBox.dataset.id = itemArray.id;
        td.appendChild(checkBox);
        tr.appendChild(td);

        allCheckBoxes.push(checkBox);

        for (let cellValue of itemArray.field) {
            const td = document.createElement('td');
            td.innerHTML = cellValue;
            tr.appendChild(td);
        }
        tbody.appendChild(tr);
    }

    table.appendChild(thead);
    table.appendChild(tbody);

    mainContainer.appendChild(table);

    checkBox.addEventListener('change', (event) => {
        for (let item of allCheckBoxes) {
            item.checked = event.target.checked;
        }
    });

    const btnDelete = createButtonDelete(checkBoxClassName,actionDeleteUsers);

    mainContainer.appendChild(btnDelete);

    return mainContainer;
}

function createButtonDelete(checkBoxClassName,actionDeleteUsers) {
    const btnDelete = document.createElement('button');
    btnDelete.textContent = 'Delete';

    btnDelete.addEventListener('click', async () => {
        const selectedBox = document.getElementsByClassName(checkBoxClassName);
        const Ids = Array.from(selectedBox)
            .filter(i => i.checked)
            .map(i => i.dataset.id);

        await actionDeleteUsers(Ids);
    });

    return btnDelete;
}

function createClickSelectAll(idMainCheckBox, checkBoxClassName) {
    const checkBox = document.getElementById(idMainCheckBox);
    const allCheckBoxes = document.getElementsByName(checkBoxClassName);

    checkBox.addEventListener('change', (event) => {
        for (let item of allCheckBoxes) {
            item.checked = event.target.checked;
        }
    });
}