const cartService = {
    getCart() {
        return JSON.parse(localStorage.getItem('cartItems')) || [];
    },
    saveCart(cart) {
        localStorage.setItem('cartItems', JSON.stringify(cart));
    },
    addItem(item) {
        const cart = this.getCart();
        const existingItem = cart.find(i => i.id === item.id);
        if (existingItem) {
            existingItem.quantity += item.quantity;
        } else {
            cart.push(item);
        }
        this.saveCart(cart);
    },
    removeItem(itemId) {
        const cart = this.getCart();
        const updatedCart = cart.filter(item => item.id !== itemId);
        this.saveCart(updatedCart);
    },
    updateItemQuantity(itemId, quantity) {
        const cart = this.getCart();
        const item = cart.find(i => i.id === itemId);
        if (item) {
            item.quantity = quantity;
            this.saveCart(cart);
        }
    },
    clearCart() {
        localStorage.removeItem('cartItems');
    },
    getTotalItems() {
        return this.getCart().reduce((total, item) => total + item.quantity, 0);
    },
    getTotalPrice() {
        return this.getCart().reduce((total, item) => {
            const price = parseFloat(item.unitPrice);
            const quantity = parseInt(item.quantity);
            if (!isNaN(price) && !isNaN(quantity)) {
                return total + price * quantity;
            }
            return total;
        }, 0);
    }
};
};

// 將 cartService 添加到全局範圍
window.cartService = cartService;