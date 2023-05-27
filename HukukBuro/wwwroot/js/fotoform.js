document.addEventListener('DOMContentLoaded', _ => {
    const foto = document.getElementById('foto');
    const fotoInput = document.getElementById('foto-input');

    console.log(foto);
    console.log(fotoInput);

    fotoInput.addEventListener('change', event => {
        const secilmisFoto = event.target.files[0];

        if (!secilmisFoto)
            return;

        const url = URL.createObjectURL(secilmisFoto);
        foto.src = url;
    });
});