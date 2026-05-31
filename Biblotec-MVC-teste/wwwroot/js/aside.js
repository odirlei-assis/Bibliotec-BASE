document.addEventListener('DOMContentLoaded', () => {
    iniciarMenuLateral();
});

function iniciarMenuLateral() {
    const menu = document.querySelector('.menu_lateral');
    const seta = document.querySelector('#setaMenu');
    const sombra = document.querySelector('.sombra');

    if (!menu || !seta || !sombra) return;

    seta.addEventListener('click', AbrirFecharMenu);

    ajustarMenuResponsivo();

    window.addEventListener('resize', ajustarMenuResponsivo);

    function AbrirFecharMenu() {
        menu.classList.toggle('menu_lateral_fechado');
        seta.classList.toggle('div_seta_mf');
        sombra.classList.toggle('movimento_sombra');
    }

    function ajustarMenuResponsivo() {
        if (window.innerWidth <= 1024) {
            menu.classList.add('menu_lateral_fechado');
            seta.classList.add('div_seta_mf');
            sombra.classList.add('movimento_sombra');
        } else {
            menu.classList.remove('menu_lateral_fechado');
            seta.classList.remove('div_seta_mf');
            sombra.classList.remove('movimento_sombra');
        }
    }
}