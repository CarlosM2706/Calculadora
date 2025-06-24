function ocultarTodo() {
    document.getElementById('estudiantes-table').style.display = 'none';
    document.getElementById('notas-table').style.display = 'none';
    document.getElementById('promedios-table').style.display = 'none';
}

async function mostrarEstudiantes() {
    ocultarTodo();
    document.getElementById('estudiantes-table').style.display = 'block';
    await cargarEstudiantes();
}

async function mostrarNotas() {
    ocultarTodo();
    document.getElementById('notas-table').style.display = 'block';
    await cargarNotas();
}

async function mostrarPromedios() {
    console.log("➡️ mostrarPromedios llamado");
    ocultarTodo();
    document.getElementById('promedios-table').style.display = 'block';
    await cargarPromedios();
}


async function cargarEstudiantes() {
    try {
        const response = await fetch('https://localhost:7030/api/Estudiantes');
        if (!response.ok) throw new Error('Error al obtener estudiantes');
        const estudiantes = await response.json();
        const tbody = document.getElementById('tbody-estudiantes');
        tbody.innerHTML = '';
        estudiantes.forEach(est => {
            const tr = document.createElement('tr');
            tr.innerHTML = `
                <td>${est.cedula}</td>
                <td>${est.nombre}</td>
                <td>${est.apellido}</td>
                <td>${est.correo || ''}</td>
                <td>${est.genero || ''}</td>
            `;
            tbody.appendChild(tr);
        });
    } catch (error) {
        console.error('Error cargando estudiantes:', error);
    }
}

async function cargarNotas() {
    try {
        const response = await fetch('https://localhost:7030/api/Notas');
        if (!response.ok) throw new Error('Error al obtener las notas');
        const notas = await response.json();

        const tbody = document.getElementById('tbody-notas');
        tbody.innerHTML = '';

        notas.forEach(nota => {
            const tr = document.createElement('tr');
            tr.innerHTML = `
            <td>${nota.cedula}</td>
            <td>${nota.nombre}</td>
            <td>${nota.apellido}</td>
            <td>${nota.curso}</td>
            <td>${parseFloat(nota.nota).toFixed(2)}</td>
            <td>${new Date(nota.fechaRegistro).toLocaleString()}</td>
            <td><button class="btn-editar" data-id="${nota.id}">✏️ Editar</button></td>
        `;
            tbody.appendChild(tr);
        });


    } catch (error) {
        console.error('Error cargando notas:', error);
        alert('No se pudieron cargar las notas.');
    }
}


async function cargarPromedios() {
    try {
        console.log("📡 Consultando promedios..."); // 👈

        const response = await fetch('https://localhost:7030/api/Promedios');
        if (!response.ok) throw new Error('Error al obtener promedios');

        const promedios = await response.json();
        console.log("✅ Datos recibidos:", promedios); // 👈

        const tbody = document.getElementById('tbody-promedios');
        tbody.innerHTML = '';

        promedios.forEach(p => {
            const tr = document.createElement('tr');
            tr.innerHTML = `
                <td>${p.cedula}</td>
                <td>${p.nombre}</td>
                <td>${p.apellido}</td>
                <td>${p.asignatura}</td>
                <td>${parseFloat(p.promedio).toFixed(2)}</td>
                <td>${p.estado}</td>
            `;
            tbody.appendChild(tr);
        });
    } catch (error) {
        console.error('❌ Error cargando promedios:', error);
        alert('No se pudieron cargar los promedios.');
    }
}


document.getElementById('tbody-notas').addEventListener('click', function (e) {
    if (e.target.classList.contains('btn-editar')) {
        const notaId = e.target.getAttribute('data-id');
        cargarNotaParaEditar(notaId);
    }
});

async function cargarNotaParaEditar(notaId) {
    try {
        const response = await fetch(`https://localhost:7030/api/Notas/${notaId}`);
        if (!response.ok) throw new Error('No se pudo cargar la nota');

        const nota = await response.json();

        document.getElementById('cedulaInput').value = nota.cedula;
        document.getElementById('cursoInput').value = nota.cursoId;
        document.getElementById('notaInput').value = nota.nota;

        const btnGuardar = document.querySelector('#form-nueva-nota button[type="submit"]');
        btnGuardar.textContent = 'Actualizar Nota';

        document.getElementById('form-nueva-nota').setAttribute('data-edit-id', notaId);

    } catch (error) {
        alert(error.message);
    }
}


document.getElementById('form-nueva-nota').addEventListener('submit', async function (e) {
    e.preventDefault();

    const cedula = document.getElementById('cedulaInput').value;
    const cursoId = document.getElementById('cursoInput').value;
    const nota = parseFloat(document.getElementById('notaInput').value);

    const editId = this.getAttribute('data-edit-id');

    try {
        let url = 'https://localhost:7030/api/Notas';
        let method = 'POST';
        let body = JSON.stringify({ cedula_es: cedula, curso_id: cursoId, nota: nota });

        if (editId) {
            // Actualizar nota existente
            url += `/${editId}`;
            method = 'PUT';
            body = JSON.stringify({ nota: nota });
        }

        const response = await fetch(url, {
            method,
            headers: { 'Content-Type': 'application/json' },
            body,
        });

        if (!response.ok) throw new Error('Error al guardar la nota');

        // Mensaje éxito
        document.getElementById('mensaje-estado').textContent = editId ? 'Nota actualizada correctamente' : 'Nota guardada correctamente';

        // Limpiar formulario y estado edición
        this.reset();
        this.removeAttribute('data-edit-id');
        document.querySelector('#form-nueva-nota button[type="submit"]').textContent = 'Guardar Nota';

        mostrarNotas();

    } catch (error) {
        alert(error.message);
    }
});




async function cargarListaEstudiantes() {
    const response = await fetch('https://localhost:7030/api/Estudiantes');
    const estudiantes = await response.json();
    const datalist = document.getElementById('listaEstudiantes');
    datalist.innerHTML = '';

    estudiantes.forEach(est => {
        const option = document.createElement('option');
        option.value = est.cedula;
        option.textContent = `${est.nombre} ${est.apellido}`;
        datalist.appendChild(option);
    });
}

async function cargarCursos() {
    try {
        const response = await fetch('https://localhost:7030/api/Cursos');
        const cursos = await response.json();

        const select = document.getElementById('cursoInput');
        select.innerHTML = '';

        const defaultOption = document.createElement('option');
        defaultOption.value = '';
        defaultOption.textContent = '--Seleccionar Curso--';
        select.appendChild(defaultOption);

        cursos.forEach(curso => {
            const option = document.createElement('option');
            option.value = curso.id;
            option.textContent = curso.nombre;
            select.appendChild(option);
        });
    } catch (error) {
        console.error('Error al cargar cursos:', error);
    }
}


function esCedula(input) {
    return /^\d{6,10}$/.test(input.trim());
}



let debounceTimer;
document.getElementById('cedulaInput').addEventListener('input', function () {
    clearTimeout(debounceTimer);
    debounceTimer = setTimeout(async () => {
        const input = this.value.trim();
        if (input.length < 2) return;

        let url;
        if (/^\d{5,10}$/.test(input)) {
            url = `https://localhost:7030/api/Estudiantes/filtrar?cedula=${input}`;
        } else {
            url = `https://localhost:7030/api/Estudiantes/filtrar?apellido=${input}`;
        }

        try {
            const response = await fetch(url);
            const estudiantes = await response.json();

            const datalist = document.getElementById('listaEstudiantes');
            datalist.innerHTML = '';

            estudiantes.forEach(est => {
                const option = document.createElement('option');
                option.value = est.cedula;
                option.textContent = `${est.nombre} ${est.apellido}`;
                datalist.appendChild(option);
            });
        } catch (error) {
            console.error('Error al cargar estudiantes:', error);
        }
    }, 300);
});


document.getElementById('form-nueva-nota').addEventListener('submit', async function (e) {
    e.preventDefault();

    const cedula = document.getElementById('cedulaInput').value.trim();
    const cursoId = document.getElementById('cursoInput').value;
    const nota = parseFloat(document.getElementById('notaInput').value);
    const editId = this.getAttribute('data-edit-id');

    if (!cedula || !cursoId || isNaN(nota)) {
        alert("Todos los campos son obligatorios.");
        return;
    }

    try {
        let url = 'https://localhost:7030/api/Notas';
        let method = 'POST';
        let body = JSON.stringify({ cedula_es: cedula, curso_id: cursoId, nota: nota });

        if (editId) {
            url += `/${editId}`;
            method = 'PUT';
            body = JSON.stringify({ nota: nota });
        }

        const response = await fetch(url, {
            method,
            headers: { 'Content-Type': 'application/json' },
            body,
        });

        // ✅ Manejo de respuesta: intenta parsear solo si es JSON
        let message = '';
        const contentType = response.headers.get('content-type');

        if (!response.ok) {
            if (contentType && contentType.includes('application/json')) {
                const errorData = await response.json();
                message = errorData.message || 'Error desconocido';
            } else {
                message = await response.text(); // texto plano
            }
            throw new Error(message);
        }

        document.getElementById('mensaje-estado').textContent = editId
            ? '✅ Nota actualizada correctamente'
            : '✅ Nota guardada correctamente';

        this.reset();
        this.removeAttribute('data-edit-id');
        document.querySelector('#form-nueva-nota button[type="submit"]').textContent = 'Guardar Nota';
        await mostrarNotas();

    } catch (error) {
        console.error(error);
        alert('❌ Error al guardar la nota: ' + error.message);
    }
});


window.onload = () => {
    ocultarTodo();
    cargarCursos();
    cargarListaEstudiantes();
    cargarEstudiantes();
};
