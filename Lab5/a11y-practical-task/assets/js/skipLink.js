
const skipLink = document.querySelector('.skip-link');

document.addEventListener('keydown', function(event) {
    if (event.key === 'Tab') {
        skipLink.classList.add('visible');
    }
});
