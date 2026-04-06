window.onload = function () {

    
    var cards = document.querySelectorAll(".product-card");

   
    setTimeout(function () {
        for (var i = 0; i < cards.length; i++) {
            (function (card, index) {
                setTimeout(function () {
                    card.style.transition = "opacity 0.45s ease, transform 0.45s ease";
                    card.style.opacity = "1";
                    card.style.transform = "translateY(0)";
                }, index * 90);
            })(cards[i], i);
        }
    }, 100);
    var pills = document.querySelectorAll(".cat-pill");
    var currentUrl = window.location.href;

    for (var j = 0; j < pills.length; j++) {
        pills[j].classList.remove("active");
        if (currentUrl === pills[j].href) {
            pills[j].classList.add("active");
        }
    }
    var anyActive = document.querySelector(".cat-pill.active");
    if (!anyActive && pills.length > 0) {
        pills[0].classList.add("active");
    }
    var navbar = document.querySelector(".nav-bar");
    window.addEventListener("scroll", function () {
        if (window.scrollY > 60) {
            navbar.style.height = "50px";
            navbar.style.transition = "height 0.25s ease";
        } else {
            navbar.style.height = "62px";
        }
    });

};