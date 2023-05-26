document.addEventListener('DOMContentLoaded', _ => {
    const belgeyiDegistir = document.getElementById('belgeyi-degistir');
    const belgeInput = document.getElementById('belge-input');

    belgeyiDegistir.addEventListener('change', inputlariAyarla);
    inputlariAyarla();

    function inputlariAyarla() {
        if (belgeyiDegistir.checked)
            belgeInput.style.display = 'flex';
        else
            belgeInput.style.display = 'none';
    }
});