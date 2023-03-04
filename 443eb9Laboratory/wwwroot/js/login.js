import { connection, ipAddress } from "./signalr.js";
connection.on("loginAnim", loginAnim);
connection.on("registerAnim", registerAnim);
var page;
(function (page) {
    page[page["login"] = 0] = "login";
    page[page["register"] = 1] = "register";
    page[page["retrieve"] = 2] = "retrieve";
})(page || (page = {}));
let loginForm = document.querySelector('.login-form');
let registerForm = document.querySelector('.register-form');
let retrieveForm = document.querySelector('.retrieve-form');
let switchButton1 = document.querySelector('.to-register');
let switchButton2 = document.querySelector('.to-retrieve');
let currentPage = page.login;
switchButton1.addEventListener('click', switchPage);
switchButton2.addEventListener('click', switchPage);
function switchPage(event) {
    let previousPage;
    switch (currentPage) {
        case page.login:
            loginForm.style.display = 'none';
            previousPage = '登录';
            break;
        case page.register:
            registerForm.style.display = 'none';
            previousPage = '注册';
            break;
        case page.retrieve:
            retrieveForm.style.display = 'none';
            previousPage = '找回';
            break;
    }
    let target = event.target;
    switch (target.innerText) {
        case '登录':
            loginForm.style.display = 'block';
            currentPage = page.login;
            break;
        case '注册':
            registerForm.style.display = 'block';
            currentPage = page.register;
            break;
        case '找回':
            retrieveForm.style.display = 'block ';
            currentPage = page.retrieve;
            break;
    }
    target.innerText = previousPage;
}
let loginButton = document.querySelector('.login .submit');
let registerButton = document.querySelector('.register .submit');
let retrieveButton = document.querySelector('.retrieve .submit');
let sendButton = document.querySelector('.send');
loginButton.addEventListener('click', login);
loginButton.addEventListener('keydown', function (event) {
    if (event.key == 'Enter') {
        loginButton.click();
    }
});
registerButton.addEventListener('click', register);
registerButton.addEventListener('keydown', function (event) {
    if (event.key == 'Enter') {
        registerButton.click();
    }
});
retrieveButton.addEventListener('click', retrieve);
retrieveButton.addEventListener('keydown', function (event) {
    if (event.key == 'Enter') {
        retrieveButton.click();
    }
});
sendButton.addEventListener('click', sendVerifCode);
function login() {
    let username = document.querySelector('.login-form .username-input');
    let password = document.querySelector('.login-form .password-input');
    if (username.value == "" || password.value == "")
        return;
    connection.invoke("Login", username.value, password.value, ipAddress, connection.connectionId);
}
export async function loginAnim(page) {
    let loginAnim = document.querySelector('.login-anim');
    let title = document.querySelector('.login-anim .title');
    loginAnim.classList.add('login-anim-enter');
    title.classList.add('title-enter');
    await new Promise(resolve => setTimeout(resolve, 2000));
    loginAnim.classList.add('login-anim-enlarge');
    await new Promise(resolve => setTimeout(resolve, 900));
    location.href = page;
}
async function register() {
    let username = document.querySelector('.register-form .username-input');
    let password = document.querySelector('.register-form .password-input');
    let email = document.querySelector('.register-form .email-input');
    let verifCode = document.querySelector('.register-form .verif-code-input');
    if (username.value == "" || password.value == "" || email.value == "" || verifCode.value == "")
        return;
    connection.invoke('Register', username.value, password.value, email.value, verifCode.value, connection.connectionId);
}
export async function registerAnim() {
    let submit = document.querySelector('.register-form .submit');
    let mask = document.querySelector('.register-form .mask');
    mask.classList.add('mask-enter');
    await new Promise(resolve => setTimeout(resolve, 500));
    submit.innerText = '注册成功';
}
async function retrieve() {
    let submit = document.querySelector('.retrieve-form .submit');
    let mask = document.querySelector('.retrieve-form .mask');
    mask.classList.add('mask-enter');
    await new Promise(resolve => setTimeout(resolve, 500));
    submit.innerText = '提交成功';
}
async function sendVerifCode() {
    let email = document.querySelector('.register-form .email-input');
    if (email.value == "")
        return;
    connection.invoke('SendVerifCode', email.value, connection.connectionId);
    sendButton.innerText = '发送成功';
}
