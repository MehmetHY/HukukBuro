document.addEventListener('DOMContentLoaded', _ => {
    const tuzelInput = document.getElementById('tuzel-input');
    const bireyInputlari = new Array(...document.getElementsByClassName('birey'));
    const sirketInputlari = new Array(...document.getElementsByClassName('sirket'));

    tuzelInput.addEventListener('change', inputlariAyarla);
    inputlariAyarla();

    function bireyInputlariGoster() {
        bireyInputlari.forEach(elm => elm.style.display = 'flex');
        sirketInputlari.forEach(elm => elm.style.display = 'none');
    }

    function sirketInputlariGoster() {
        bireyInputlari.forEach(elm => elm.style.display = 'none');
        sirketInputlari.forEach(elm => elm.style.display = 'flex');
    }

    function inputlariAyarla() {
        if (tuzelInput.checked)
            sirketInputlariGoster();
        else
            bireyInputlariGoster();
    }
});