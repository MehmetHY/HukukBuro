document.addEventListener('DOMContentLoaded', _ => {
    const dosyaBaglantisi = document.getElementById('dosya-baglantisi');
    const kisiBaglantisi = document.getElementById('kisi-baglantisi');
    const personelBaglantisi = document.getElementById('personel-baglantisi');
    const dosyaInput = document.getElementById('dosya-input');
    const kisiInput = document.getElementById('kisi-input');
    const personelInput = document.getElementById('personel-input');

    const odendi = document.getElementById('odendi');
    const odemeTarihiInput = document.getElementById('odeme-tarihi-input');
    const sonOdemeTarihiInput = document.getElementById('son-odeme-tarihi-input');
    const makbuzKesildi = document.getElementById('makbuz-kesildi');
    const makbuzKesildiInput = document.getElementById('makbuz-kesildi-input');
    const makbuzNoInput = document.getElementById('makbuz-no-input');
    const makbuzTarihiInput = document.getElementById('makbuz-tarihi-input');

    dosyaBaglantisi.addEventListener('change', inputlariAyarla);
    kisiBaglantisi.addEventListener('change', kisiInputAyarla);
    personelBaglantisi.addEventListener('change', personelInputAyarla);
    odendi.addEventListener('change', odemeInputAyarla);
    makbuzKesildi.addEventListener('change', makbuzInputAyarla);

    inputlariAyarla();

    function dosyaInputAyarla() {
        if (dosyaBaglantisi.checked)
            dosyaInput.style.display = 'flex';
        else
            dosyaInput.style.display = 'none';
    }

    function kisiInputAyarla() {
        if (kisiBaglantisi.checked)
            kisiInput.style.display = 'flex';
        else
            kisiInput.style.display = 'none';
    }

    function personelInputAyarla() {
        if (personelBaglantisi.checked)
            personelInput.style.display = 'flex';
        else
            personelInput.style.display = 'none';
    }

    function makbuzInputAyarla() {
        if (makbuzKesildi.checked) {
            makbuzNoInput.style.display = 'flex';
            makbuzTarihiInput.style.display = 'flex';
        }
        else {
            makbuzNoInput.style.display = 'none';
            makbuzTarihiInput.style.display = 'none';
        }
    }

    function odemeInputAyarla() {
        if (odendi.checked) {
            odemeTarihiInput.style.display = 'flex';
            makbuzKesildiInput.style.display = 'flex';
            sonOdemeTarihiInput.style.display = 'none';
            makbuzInputAyarla();
        }
        else {
            odemeTarihiInput.style.display = 'none';
            makbuzKesildiInput.style.display = 'none';
            makbuzNoInput.style.display = 'none';
            makbuzTarihiInput.style.display = 'none';
            sonOdemeTarihiInput.style.display = 'flex';
        }
    }

    function inputlariAyarla() {
        dosyaInputAyarla();
        kisiInputAyarla();
        personelInputAyarla();
        odemeInputAyarla();
    }
});