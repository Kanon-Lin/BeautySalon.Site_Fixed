const useAuth = () => {
    const user = Vue.ref(authService.getCurrentUser());
    const isAuthenticated = Vue.computed(() => authService.isAuthenticated());

    //#region 認證相關方法
    const login = async (credentials) => {
        const result = await authService.login(credentials);
        if (result.success) {
            user.value = result.user;
            // 登入後，購物車數據保持不變
        }
        return result;
    };

    const logout = () => {
        authService.logout();
        user.value = null;
        clearCart();
    };

    const getUserProfile = async () => {
        try {
            console.log('Fetching user profile...');
            const profile = await authService.getUserProfile();
            console.log('Profile received:', profile);
            user.value = { ...user.value, ...profile };
            return profile;
        } catch (error) {
            console.error('Error fetching user profile:', error);
            throw error;
        }
    };

    const updateUserProfile = async (profileData) => {
        try {
            return await authService.updateUserProfile(profileData);
        } catch (error) {
            console.error('Error updating user profile:', error);
            throw error;
        }
    };

    const changePassword = async (passwordData) => {
        try {
            return await authService.changePassword(passwordData);
        } catch (error) {
            console.error('Error changing password:', error);
            throw error;
        }
    };
    //#endregion

    //#region 購物車相關方法
    const getCart = () => {
        const cart = JSON.parse(localStorage.getItem('cartItems')) || [];
        console.log('Retrieved cart:', cart); // 添加日誌
        return cart;
    };

    const saveCart = (cart) => {
        localStorage.setItem('cartItems', JSON.stringify(cart));
    };

    const addToCart = (item) => {
        const cart = getCart();
        const existingItem = cart.find(i => i.id === item.id);
        if (existingItem) {
            existingItem.quantity += item.quantity;
        } else {
            cart.push(item);
        }
        saveCart(cart);
        console.log('Item added to cart:', item); // 添加日誌
        console.log('Current cart:', getCart()); // 添加日誌
    };

    const removeFromCart = (itemId) => {
        const cart = getCart();
        const updatedCart = cart.filter(item => item.id !== itemId);
        saveCart(updatedCart);
    };

    const updateCartItemQuantity = (itemId, quantity) => {
        const cart = getCart();
        const item = cart.find(i => i.id === itemId);
        if (item) {
            item.quantity = quantity;
            saveCart(cart);
        }
    };

    const clearCart = () => {
        localStorage.removeItem('cartItems');
    };

    const getTotalCartItems = () => {
        return getCart().reduce((total, item) => total + item.quantity, 0);
    };

    const getTotalCartPrice = () => {
        return getCart().reduce((total, item) => total + item.price * item.quantity, 0);
    };

    //#endregion

    //#region 訂單相關方法
    const getOrders = async () => {
        try {
            return await orderService.getOrderList();
        } catch (error) {
            console.error('Error fetching orders:', error);
            throw error;
        }
    };

    const getOrderDetails = async (orderId) => {
        try {
            return await orderService.getOrderDetails(orderId);
        } catch (error) {
            console.error('Error fetching order details:', error);
            throw error;
        }
    };

    const createOrder = async (orderData) => {
        try {
            console.log('Creating order with data:', orderData);
            const result = await orderService.createOrder(orderData);
            console.log('Order creation result:', result);
            if (result.success) {
                clearCart(); // 創建訂單成功後清空購物車
            }
            return result;
        } catch (error) {
            console.error('Error in createOrder:', error);
            throw error;
        }
    };

    const cancelOrder = async (orderId) => {
        try {
            return await orderService.cancelOrder(orderId);
        } catch (error) {
            console.error('Error cancelling order:', error);
            throw error;
        }
    };
    //#endregion

    return {
        //認證相關方法
        user,
        isAuthenticated,
        login,
        logout,
        getUserProfile,
        updateUserProfile,
        changePassword,
        // 購物車相關方法
        getCart,
        addToCart,
        removeFromCart,
        updateCartItemQuantity,
        clearCart,
        getTotalCartItems,
        getTotalCartPrice,
        // 訂單相關方法
        getOrders,
        getOrderDetails,
        createOrder,
        cancelOrder
    };
};
window.useAuth = useAuth;