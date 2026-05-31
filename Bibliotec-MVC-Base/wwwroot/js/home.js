// const nav = document.querySelector('nav');
// const nav = document.querySelector('.nav');
// const nav = document.querySelector('#nav');

const nav = document.getElementById('nav_header');
function AbrirFecharMenu(){
    // console.log(nav);

    if (window.getComputedStyle(nav).top == "-275px" ) {
        nav.style.top = "96px";
    } else {
        nav.style.top = "-275px";
    }
}