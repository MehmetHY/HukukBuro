document.addEventListener('DOMContentLoaded', _ => {
    const anarol = document.getElementById('anarol');
    const yetkilerBaslik = document.getElementById('yetkiler-baslik');
    const yetkilerInput = document.getElementById('yetkiler-input');
    const yoneticiIndex = 1;

    anarol.addEventListener('change', InputlariAyarla);
    InputlariAyarla();

    function InputlariAyarla() {
        if (anarol.selectedIndex === yoneticiIndex) {
            yetkilerBaslik.style.display = 'block';
            yetkilerInput.style.display = 'flex';
        }
        else {
            yetkilerBaslik.style.display = 'none';
            yetkilerInput.style.display = 'none';
        }
    }
});