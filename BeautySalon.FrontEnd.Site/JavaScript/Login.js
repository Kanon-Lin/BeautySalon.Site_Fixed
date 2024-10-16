const { createApp, ref } = Vue;

createApp({
    setup() {
        const account = ref("");
        const password = ref("");
        const email = ref("");
        const errorMessage = ref("")
        const isForgotPasswordVisible = ref(false);

        const login = () => {
            const loginData = {
                account: account.value,
                password: password.value,
            };
            fetch('/api/MembersApi/Login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(loginData),
            })
                .then(response => {
                    if (!response.ok) {
                        if (response.status === 500) {
                            throw new Error('伺服器內部錯誤，請聯繫管理員');
                        }
                        return response.json().then(err => {
                            throw new Error(err.message || `錯誤 ${response.status}: ${response.statusText || '未知錯誤'}`);
                        });
                    }
                    return response.json();
                })
                .then(data => {
                    if (data.success && data.token) {
                        localStorage.setItem("token", data.token);
                        localStorage.setItem("userInfo", JSON.stringify({ name: data.memberName }));
                        alert('登入成功');
                        window.location.href = "/Html/HomePage.html";
                    } else {
                        errorMessage.value = data.message || "登入失敗，請檢查您的帳號和密碼";
                    }
                })
                .catch(error => {
                    console.error('登入錯誤:', error);
                    errorMessage.value = error.message || "發生錯誤，請稍後再試";
                });
        };
    },

        const showForgotPasswordModal= () => {
            isForgotPasswordVisible.value = true;
        },

        const closeForgotPasswordModal = () => {
            isForgotPasswordVisible.value = false;
            email.value = "";
        },

        const sendResetLink = () => {
            if (email.value) {
                alert("密碼重設連結已發送到您的電子郵件！");
                closeForgotPasswordModal();
            } else {
                alert("請輸入電子郵件！");
            }
        },

        return {
            account,
            password,
            email,
            errorMessage,
            isForgotPasswordVisible,
            login,
            showForgotPasswordModal,
            closeForgotPasswordModal,
            sendResetLink,
        },
    },
}).mount("#app");
