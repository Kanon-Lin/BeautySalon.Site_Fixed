const userService = {
    async getUserProfile() {
        return authService.getUserProfile();
    },
    async updateUserProfile(profileData) {
        return authService.updateUserProfile(profileData);
    },
    // 其他用戶相關方法
};
window.userService = userService;