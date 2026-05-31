document.addEventListener('DOMContentLoaded', () => {
    const menu = document.querySelector('.menu_lateral');
    const setaMenu = document.querySelector('#setaMenu');
    const sombra = document.querySelector('.sombra');

    if(setaMenu) {
        setaMenu.addEventListener('click', () => {
            if(menu) menu.classList.toggle('menu_lateral_fechado');
            if(setaMenu) setaMenu.classList.toggle('div_seta_mf');
            if(sombra) sombra.classList.toggle('movimento_sombra');
        });
    }

    function ajustarMenuResponsivo() {
        if (window.innerWidth <= 1024) {
            if(menu) menu.classList.add('menu_lateral_fechado');
            if(setaMenu) setaMenu.classList.add('div_seta_mf');
            if(sombra) sombra.classList.add('movimento_sombra');
        } else {
            if(menu) menu.classList.remove('menu_lateral_fechado');
            if(setaMenu) setaMenu.classList.remove('div_seta_mf');
            if(sombra) sombra.classList.remove('movimento_sombra');
        }
    }

    ajustarMenuResponsivo();
    window.addEventListener('resize', ajustarMenuResponsivo);
});

function AbrirFecharMenu() {
    const menu = document.querySelector('.menu_lateral');
    const setaMenu = document.querySelector('#setaMenu');
    const sombra = document.querySelector('.sombra');
    
    if(menu) menu.classList.toggle('menu_lateral_fechado');
    if(setaMenu) setaMenu.classList.toggle('div_seta_mf');
    if(sombra) sombra.classList.toggle('movimento_sombra');
}