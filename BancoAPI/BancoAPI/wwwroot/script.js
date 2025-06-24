const apiBase = "https://localhost:7130/api";

function verificarCuenta() {
    const cuenta = document.getElementById("cuenta").value.trim();
    const estado = document.getElementById("estado");
    const transferenciaDiv = document.getElementById("transferencia");

    if (cuenta === "") {
        estado.textContent = "⚠️ Ingrese un número de cuenta.";
        estado.style.color = "orange";
        return;
    }

    fetch(`${apiBase}/banco/cuenta-existe/${cuenta}`)
        .then(res => res.json())
        .then(data => {
            if (data.existe) {
                estado.textContent = "✅ Cuenta válida";
                estado.style.color = "green";
                transferenciaDiv.classList.remove("hidden");
            } else {
                estado.textContent = "❌ La cuenta no existe";
                estado.style.color = "red";
                transferenciaDiv.classList.add("hidden");
            }
        })
        .catch(err => {
            estado.textContent = "❌ Error en la verificación";
            estado.style.color = "red";
        });
}

function realizarTransferencia() {
    const cuenta = document.getElementById("cuenta").value.trim();
    const destino = document.getElementById("destino").value.trim();
    const valor = parseFloat(document.getElementById("valor").value);
    const resultado = document.getElementById("resultado");

    if (!cuenta || !destino || isNaN(valor) || valor <= 0) {
        resultado.textContent = "⚠️ Complete todos los campos con datos válidos.";
        resultado.style.color = "orange";
        return;
    }

    const transferencia = {
        cuentaOrigen: cuenta,
        cuentaDestino: destino,
        valor: valor
    };

    fetch(`${apiBase}/banco/transferir`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(transferencia)
    })
        .then(res => {
            if (!res.ok) return res.json().then(err => { throw err; });
            return res.json();
        })
        .then(data => {
            resultado.textContent = "✅ Transferencia realizada con éxito.";
            resultado.style.color = "green";
        })
        .catch(err => {
            resultado.textContent = "❌ " + (err.error || "Error en la transferencia.");
            resultado.style.color = "red";
        });
}
