function createTable(data) {
    const table = document.createElement('table');

    const thead = document.createElement('thead');
    const headRow = document.createElement('tr');
    const tbody = document.createElement('tbody');

    for (let item of data.head) {
        const th = document.createElement('th');
        th.textContent = item;
        headRow.appendChild(th);
    }
    thead.appendChild(headRow);

    for (let itemArray of data.body) {
        const tr = document.createElement('tr');
        for (let cellValue of itemArray) {
            const td = document.createElement('td');
            td.innerHTML = cellValue;
            tr.appendChild(td);
        }
        tbody.appendChild(tr);
    }

    table.appendChild(thead);
    table.appendChild(tbody);
    return table;
}