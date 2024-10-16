// validators.js

export const validateName = (name) => {
    if (!name || name.trim().length === 0) {
        return "*請輸入姓名";
    }
    if (name.length > 50) {
        return "*姓名不能超過50個字符";
    }
    return "";
};

export const validateEmail = (email) => {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!email || email.trim().length === 0) {
        return "*請輸入電子郵件";
    }
    if (!emailRegex.test(email)) {
        return "請輸入有效的電子郵件地址";
    }
    return "";
};

export const validatePhone = (phone) => {
    const phoneRegex = /^[0-9]{10}$/;
    if (!phone || phone.trim().length === 0) {
        return "*請輸入電話號碼";
    }
    if (!phoneRegex.test(phone)) {
        return "*請輸入有效的10位電話號碼";
    }
    return "";
};

export const validatePassword = (password) => {
    if (!password || password.length < 8) {
        return "*密碼必須至少8個字符";
    }
    if (
        !/[A-Z]/.test(password) ||
        !/[a-z]/.test(password) ||
        !/[0-9]/.test(password)
    ) {
        return "*密碼必須包含大小寫字母和數字";
    }
    return "";
};

export const validatePasswordConfirmation = (password, confirmPassword) => {
    if (password !== confirmPassword) {
        return "*兩次輸入的密碼不匹配";
    }
    return "";
};
