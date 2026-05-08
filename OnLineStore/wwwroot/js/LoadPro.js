const baseURL = "/ECommerce/Home/Data";
const newProduct = document.getElementById("newProducts");
const btnContainer = document.getElementById("btnContainer");
const btns = document.querySelectorAll(".mybtn");
let products = [];
let index = 0;
let pages = [];
const searchBar = document.getElementById("searchBar");
searchBar.addEventListener("keyup", function (e) {
    const searchString = e.target.value.toLowerCase();
    const filterProducts = products.filter((product) => {
        return (
            product.name.toLowerCase().includes(searchString) ||
            product.type.toLowerCase().includes(searchString)
        );
    });
    displayProducts(filterProducts);
});
//const search
for (let i = 0; i < btns.length; i++) {
    btns[i].addEventListener("click", (e) => {
        e.preventDefault();
        const filter = e.target.dataset.filter;
        console.log(filter);
        const filterProducts = products.filter((product) => {
            if (filter === "كل الفئات") {
                return product.type.toLowerCase();
            }
            return product.type.toLowerCase().includes(filter);
        });
        displayProducts(filterProducts);
    });
}
//<ul class="icons">
//    <li>
//        <a href="/ECommerce/Home/AddToCart/${product.id}">
//            <i class="fa fa-shopping-bag"></i>
//        </a>
//    </li>
//</ul>
const displayProducts = (products) => {
    const htmlTemplate = products
        .map((product) => {
            return `
            <div class="col-md-4">
                        <div class="card pro">
                            <div class="cardHeader">                               
                                <img class="card-img-top" src="${product.img}" alt="card image">
                            </div>
                            <div class="card-body">
                                <h6>${product.name}</h6>
                                <h7>${product.type}</h7>
                                <h7 class="price">السعر: ${product.price} رس</h7>
                                <a href="/ECommerce/Home/Detail/${product.id}" class = "btn">تفاصيل المنتج</a>                                
                            </div>
                        </div>
                    </div>
                    `;
        })
        .join("");
    newProduct.innerHTML = htmlTemplate;
};

const loadProducts = async () => {
    try {
        const response = await fetch(baseURL);
        products = await response.json();
        products = products.data;
        console.log(products);
        displayProducts(products);
        if (!response.ok)
            throw new Error(`${products.message} ${response.status}`);
        return products;
    } catch (err) {
        console.log(err);
    }
};

const paginate = (products) => {
    const pageItems = 9;
    const pagesCount = Math.ceil(products.length / pageItems);
    const newProducts = Array.from({ length: pagesCount }, (_, pageindex) => {
        const start = pageindex * pageItems;
        return products.slice(start, start + pageItems);
    });
    return newProducts;
};
const displayButtons = (btnContainer, pages, activeIndex) => {
    let btns = pages.map((_, pageIndex) => {
        return `<button class="pageBtn 
        ${activeIndex === pageIndex ? "activeBtn" : "null"}
        " data-index= "${pageIndex}">
        ${pageIndex + 1}</button>`;
    });
    btns.push(`<button class="prevBtn">
    <i class="fa fa-arrow-left" aria-hidden="true"></i></button>`);
    btnContainer.innerHTML = btns.join("");
};

btnContainer.addEventListener("click", function (e) {
    if (e.target.classList.contains("btnContainer")) return;
    if (e.target.classList.contains("pageBtn")) {
        index = parseInt(e.target.dataset.index);
    }
    if (e.target.classList.contains("nextBtn")) {
        index++;
        if (index > pages.length - 1) {
            index = 0;
        }
    }
    if (e.target.classList.contains("prevBtn")) {
        index--;
        if (index < 0) {
            index = pages.length - 1;
        }
    }
    displayProducts(pages[index]);
    displayButtons(btnContainer, pages, index);
});

window.addEventListener("load", async () => {
    const products = await loadProducts();
    pages = paginate(products);
    displayProducts(pages[index]);
    displayButtons(btnContainer, pages, index);
});
