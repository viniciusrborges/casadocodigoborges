(function () {
    const colecaoLinks = document.querySelectorAll(".colecoesDaCDC-colecaoLink--comSubColecao");
    colecaoLinks.forEach(function (item) {
        item.addEventListener("mouseover", function (e) {
            const mq = window.matchMedia("(min-width: 620px)");
            if (mq.matches) {
                abreMenu(e.target);
            }
        });

        item.addEventListener("click", function (e) {
            const mq = window.matchMedia("not all and (min-width: 620px)");
            if (mq.matches) {
                e.preventDefault();
                if (e.target.parentNode.classList.contains("colecoesDaCDC-colecaoItem--ativo")) {
                    fechaMenu(e.target.parentNode);
                }
                else {
                    abreMenu(e.target);
                }
            }
        });

        item.parentNode.addEventListener("mouseleave", function (e) {
            const mq = window.matchMedia("(min-width: 620px)");
            if (mq.matches) {
                fechaMenu(e.target);
            }
        })
    });

    function fechaMenu(target) {
        slideUp(target);

        target.classList.remove("colecoesDaCDC-colecaoItem--ativo");
    }

    function abreMenu(target) {
        const colecoes = Array.from(document.querySelectorAll(".colecoesDaCDC-colecaoItem"));
        const colecaoItemClicado = target.parentNode;

        colecoes.forEach(slideUp);

        slideDown(colecaoItemClicado);

        colecoes.map(menuItem => menuItem.classList.remove("colecoesDaCDC-colecaoItem--ativo"));

        colecaoItemClicado.classList.add("colecoesDaCDC-colecaoItem--ativo");
    }

    function getSubColecoes(menu) {
        return menu.querySelector(".colecoesDaCDC-colecaoItem-subColecoes");
    }

    function descobreAltura(menu) {
        menu.style.display = "block";
        const altura = menu.scrollHeight;
        menu.style.display = "none";
        return altura;
    };

    function slideUp(colecaoClicada) {
        const menu = getSubColecoes(colecaoClicada);
        if (menu) {
            menu.style.maxHeight = "0";
        }
    };

    function slideDown(colecaoClicada) {
        const menu = getSubColecoes(colecaoClicada);
        if (menu) {
            const maxHeight = menu.getAttribute('data-altura');
            if (maxHeight) {
                menu.style.maxHeight = maxHeight;
            }
            else {
                const altura = descobreAltura(menu) + "px";
                menu.style.maxHeight = "0";
                menu.style.display = "block";
                menu.setAttribute("data-altura", altura);
                setTimeout(function () {
                    menu.style.maxHeight = altura;
                }, 10);
            }
        }
    };
})();