document.addEventListener('DOMContentLoaded', _ => {
    const baglanti = document.getElementById('baglanti');
    const kisiInput = document.getElementById('kisi-input');
    const dosyaInput = document.getElementById('dosya-input');

    console.log(baglanti);

    baglanti.addEventListener('change', inputlariAyarla);
    inputlariAyarla();

    function inputlariAyarla() {
        let kisiDisplay, dosyaDisplay;

        switch (baglanti.selectedIndex) {
            case 1:
                kisiDisplay = 'none';
                dosyaDisplay = 'flex';
                break;
            case 2:
                kisiDisplay = 'flex';
                dosyaDisplay = 'none';
                break;
            default:
                kisiDisplay = 'none';
                dosyaDisplay = 'none';
        }

        kisiInput.style.display = kisiDisplay;
        dosyaInput.style.display = dosyaDisplay;
    }
});