import { Announcement } from "ETCC/etcc-datamodels.js"
import { connection, ipAddress } from "./signalr.js"

connection.on("loginAnim", loginAnim)
connection.on("registerAnim", registerAnim)
connection.on("getAnnouncementInfo", getAnnouncementInfo)

enum page {
    login,
    register,
    retrieve
}

connection.invoke('SendAnnouncementInfo', connection.connectionId)

let articleContainer = document.querySelector('.article-container ul') as HTMLElement

export function getAnnouncementInfo(json: string) {
    let announcement = JSON.parse(json) as Announcement[]
    announcement.forEach(ann => {
        articleContainer.insertAdjacentHTML('afterbegin',
            `<li>
            <div class="ann-article">
                <h2 class="title">`+ ann.title + `</h2>
                <p class="content">
                    `+ ann.content + `
                </p>
            </div>
        </li>
        `)
    });
}

let loginForm = document.querySelector('.login-form') as HTMLDivElement
let registerForm = document.querySelector('.register-form') as HTMLDivElement
let retrieveForm = document.querySelector('.retrieve-form') as HTMLDivElement

let switchButton1 = document.querySelector('.to-register') as HTMLButtonElement
let switchButton2 = document.querySelector('.to-retrieve') as HTMLButtonElement

let currentPage = page.login

switchButton1.addEventListener('click', switchPage)
switchButton2.addEventListener('click', switchPage)

function switchPage(event: MouseEvent) {
    let previousPage: string
    switch (currentPage) {
        case page.login:
            loginForm.style.display = 'none'
            previousPage = '登录'
            break
        case page.register:
            registerForm.style.display = 'none'
            previousPage = '注册'
            break
        case page.retrieve:
            retrieveForm.style.display = 'none'
            previousPage = '找回'
            break
    }

    let target = event.target as HTMLButtonElement
    switch (target.innerText) {
        case '登录':
            loginForm.style.display = 'block'
            currentPage = page.login
            break
        case '注册':
            registerForm.style.display = 'block'
            currentPage = page.register
            break
        case '找回':
            retrieveForm.style.display = 'block '
            currentPage = page.retrieve
            break
    }
    target.innerText = previousPage
}

let loginButton = document.querySelector('.login .submit') as HTMLButtonElement
let registerButton = document.querySelector('.register .submit') as HTMLButtonElement
let retrieveButton = document.querySelector('.retrieve .submit') as HTMLButtonElement
let sendButton = document.querySelector('.send') as HTMLButtonElement

loginButton.addEventListener('click', login)
loginButton.addEventListener('keydown', function (event) {
    if (event.key == 'Enter') {
        loginButton.click()
    }
})
registerButton.addEventListener('click', register)
registerButton.addEventListener('keydown', function (event) {
    if (event.key == 'Enter') {
        registerButton.click()
    }
})
retrieveButton.addEventListener('click', retrieve)
retrieveButton.addEventListener('keydown', function (event) {
    if (event.key == 'Enter') {
        retrieveButton.click()
    }
})

sendButton.addEventListener('click', sendVerifCode)

function login() {
    let username = document.querySelector('.login-form .username-input') as HTMLInputElement
    let password = document.querySelector('.login-form .password-input') as HTMLInputElement
    if (username.value == "" || password.value == "") return

    connection.invoke("Login", username.value, password.value, ipAddress, connection.connectionId)
}

export async function loginAnim(page: string) {
    let loginAnim = document.querySelector('.login-anim') as HTMLDivElement
    let title = document.querySelector('.login-anim .title') as HTMLTitleElement

    loginAnim.classList.add('login-anim-enter')
    title.classList.add('title-enter')
    await new Promise(resolve => setTimeout(resolve, 2000))

    loginAnim.classList.add('login-anim-enlarge')
    await new Promise(resolve => setTimeout(resolve, 900))
    location.href = page
}

async function register() {
    let username = document.querySelector('.register-form .username-input') as HTMLInputElement
    let password = document.querySelector('.register-form .password-input') as HTMLInputElement
    let email = document.querySelector('.register-form .email-input') as HTMLInputElement
    let verifCode = document.querySelector('.register-form .verif-code-input') as HTMLInputElement
    if (username.value == "" || password.value == "" || email.value == "" || verifCode.value == "") return

    connection.invoke('Register', username.value, password.value, email.value, verifCode.value, connection.connectionId)
}

export async function registerAnim() {
    let submit = document.querySelector('.register-form .submit') as HTMLButtonElement
    let mask = document.querySelector('.register-form .mask') as HTMLDivElement

    mask.classList.add('mask-enter')
    await new Promise(resolve => setTimeout(resolve, 500))
    submit.innerText = '注册成功'
}

async function retrieve() {
    let submit = document.querySelector('.retrieve-form .submit') as HTMLButtonElement
    let mask = document.querySelector('.retrieve-form .mask') as HTMLDivElement

    mask.classList.add('mask-enter')
    await new Promise(resolve => setTimeout(resolve, 500))
    submit.innerText = '提交成功'
}

async function sendVerifCode() {
    let email = document.querySelector('.register-form .email-input') as HTMLInputElement
    if (email.value == "") return

    connection.invoke('SendVerifCode', email.value, connection.connectionId)
    sendButton.innerText = '发送成功'
}