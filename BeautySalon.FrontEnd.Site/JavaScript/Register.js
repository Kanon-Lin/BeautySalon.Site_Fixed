const { createApp, ref } = Vue;

const app = createApp({
    components: {
        "navbar-component": NavbarComponent,
    },
    setup() {
        const account = ref("");
        const password = ref("");
        const email = ref("");
        const name = ref("");
        const gender = ref(0);  // 假設默認值為0
        const mobile = ref("");
        const errorMessage = ref("");

        const validateInput = () => {
            if (!email.value.includes("@")) {
                errorMessage.value = "請輸入有效的電子郵件地址";
                return false;
            }
            return true;
        };

        const register = () => {
            if (!validateInput()) return;
            const registerData = {
                Account: account.value,
                Password: password.value,
                Email: email.value,
                Name: name.value,
                Gender: gender.value,
                PhoneNumber: mobile.value
            };
            console.log(registerData); // 輸出提交的資料


            fetch('/api/MembersApi/Register', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(registerData)
            })
                .then(response => {
                    if (!response.ok) {
                        return response.text().then(text => {
                            throw new Error(text || '伺服器錯誤');
                        });
                    }
                    const contentType = response.headers.get('content-type');
                    if (contentType && contentType.includes('application/json')) {
                        return response.json();
                    } else {
                        return response.text().then(text => {
                            return { Success: true, Message: text };
                        });
                    }
                })
                .then(data => {
                    console.log("data:", data);
                    if (data.Success) {
                        alert("註冊成功，請查收您的電子郵件進行驗證！");
                        window.location.href = '/Html/HomePage.html';
                    } else {
                        errorMessage.value = data.Message || "註冊失敗，請再試一次";
                    }
                })
                .catch(error => {
                    console.error('Error:', error);
                    errorMessage.value = error.message || "伺服器發生錯誤，請稍後再試";
                });
        };
        const autoFill = () => {
            account.value = "testuser";
            password.value = "0123456";
            email.value = "testuser@gmail.com";
            name.value = "TestUser";
            gender.value = "0";
            mobile.value = "0998663555";
        };

        return {
            account,
            password,
            email,
            name,
            gender,
            mobile,
            errorMessage,
            register,
            autoFill
        };
    },
}).mount("#app");
