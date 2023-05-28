document.addEventListener('DOMContentLoaded', _ => {
    const sifreDegistir = document.getElementById('sifre-degistir');
    const sifreInputlari = new Array(...document.getElementsByClassName('sifre-input'));

    sifreDegistir.addEventListener('change', inputlariAyarla);
    inputlariAyarla();

    function inputlariAyarla() {
        if (sifreDegistir.checked)
            sifreInputlari.forEach(elm => elm.style.display = 'flex');
        else
            sifreInputlari.forEach(elm => elm.style.display = 'none');
    }
});