const orderService = (() => {
    const getAuthHeaders = () => {
        const token = authService.getTokenFromCookie();
        return {
            'Authorization': `Bearer ${token}`,
            'Content-Type': 'application/json'
        };
    };

    const handleResponse = async (response) => {
        if (!response.ok) {
            const errorData = await response.json();
            throw new Error(errorData.message || 'API request failed');
        }
        return await response.json();
    };

    return {
        async getOrders() {
            try {
                const response = await fetch('/api/OrdersApi', {
                    method: 'GET',
                    headers: getAuthHeaders()
                });
                return await handleResponse(response);
            } catch (error) {
                console.error('Error in getOrderList:', error);
                throw error;
            }
        },

        async getOrderDetails(orderId) {
            try {
                console.log(`Fetching order details for order ID: ${orderId}`); // 添加日誌
                const response = await fetch(`/api/OrdersApi/${orderId}`, {
                    method: 'GET',
                    headers: getAuthHeaders()
                });
                console.log('Response status:', response.status); // 添加日誌
                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }
                const data = await response.json();
                console.log('Received order details:', data); // 添加日誌
                return data;
            } catch (error) {
                console.error('Error in getOrderDetails:', error);
                throw error;
            }
        },

        async createOrder(orderData) {
            try {
                const response = await fetch('/api/OrdersApi', {
                    method: 'POST',
                    headers: getAuthHeaders(),
                    body: JSON.stringify(orderData)
                });
                return await handleResponse(response);
            } catch (error) {
                console.error('Error in createOrder:', error);
                throw error;
            }
        },

        async cancelOrder(orderId) {
            try {
                console.log(`Cancelling order with ID: ${orderId}`); // 添加日誌
                const response = await fetch(`/api/OrdersApi/${orderId}/cancel`, {
                    method: 'POST',
                    headers: getAuthHeaders()
                });
                console.log('Cancel order response status:', response.status); // 添加日誌

                const result = await handleResponse(response);
                console.log('Cancel order result:', result); // 添加日誌

                if (result.success) {
                    return {
                        success: true,
                        message: result.message,
                        newStatus: result.newStatus
                    };
                } else {
                    throw new Error(result.message || '取消訂單失敗');
                }
            } catch (error) {
                console.error('Error in cancelOrder:', error);
                throw error;
            }
        }
    };
})();

window.orderService = orderService;