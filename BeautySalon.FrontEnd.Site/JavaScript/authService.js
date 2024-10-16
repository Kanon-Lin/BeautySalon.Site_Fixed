// authService.js

const authService = {
    // Token 管理
    setTokenCookie(token, expiryMonth = 6) {
        const d = new Date();
        d.setTime(d.getTime() + (expiryMonth * 30 * 24 * 60 * 60 * 1000));
        const expires = "expires=" + d.toUTCString();
        document.cookie = `token=${token}; ${expires}; path=/; Secure; SameSite=Strict`;
    },

    getTokenFromCookie() {
        const name = "token=";
        const decodedCookie = decodeURIComponent(document.cookie);
        const ca = decodedCookie.split(';');
        for (let i = 0; i < ca.length; i++) {
            let c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) == 0) {
                return c.substring(name.length, c.length);
            }
        }
        return "";
    },

    clearTokenCookie() {
        document.cookie = "token=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
    },

    // 用戶認證
    async login(credentials) {
        try {
            const response = await fetch('/api/MembersApi/Login', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(credentials)
            });

            const data = await response.json();
            console.log('Login response:', data);

            if (response.ok && data.success && data.token) {
                this.setTokenCookie(data.token);
                localStorage.setItem("userInfo", JSON.stringify({ name: data.memberName }));
                console.log('Login successful, user info set');
                return { success: true, user: { name: data.memberName } };
            } else if (response.status === 500) {
                console.error('Server error during login');
                return { success: false, message: "伺服器錯誤,請聯繫管理員" };
            } else {
                console.error('Login failed:', data.Message || 'Unknown error');
                return { success: false, message: data.Message || "登入失敗,請檢查您的憑證" };
            }
        } catch (error) {
            console.error('Login error:', error);
            return { success: false, message: "網絡錯誤,請檢查您的連接並稍後再試" };
        }
    },

    logout() {
        this.clearTokenCookie();
        localStorage.removeItem('userInfo');
    },

    isAuthenticated() {
        return !!this.getTokenFromCookie();
    },

    getCurrentUser() {
        const userInfo = localStorage.getItem('userInfo');
        return userInfo ? JSON.parse(userInfo) : null;
    },

    // 用戶資料管理
    async getUserProfile() {
        const token = this.getTokenFromCookie();
        try {
            const response = await fetch('/api/MembersApi/Profile', {
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            });
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            const data = await response.json();
            console.log('User profile data:', data);
            return data;
        } catch (error) {
            console.error('Error fetching user profile:', error);
            throw error;
        }
    },

    async updateUserProfile(profileData) {
        const token = this.getTokenFromCookie();
        try {
            const response = await fetch('/api/MembersApi/Profile', {
                method: 'PUT',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(profileData)
            });
            if (response.ok) {
                return await response.json();
            }
            throw new Error('Failed to update user profile');
        } catch (error) {
            console.error('Error updating user profile:', error);
            throw error;
        }
    },


    async changePassword(passwordData) {
        const token = this.getTokenFromCookie();
        try {
            const response = await fetch('/api/MembersApi/Password', {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${token}`,
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(passwordData)
            });
            const data = await response.json();
            if (response.ok) {
                return { success: true, message: data.Message };
            }
            throw new Error(data.Message || data.Errors?.join(', ') || '更改密碼失敗');
        } catch (error) {
            console.error('Error changing password:', error);
            throw error;
        }
    }
};

//確保 authService 可以全局訪問
window.authService = authService;