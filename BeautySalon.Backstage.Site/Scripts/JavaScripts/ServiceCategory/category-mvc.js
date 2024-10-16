const { createApp, ref, onMounted } = Vue;

const config = {
    setup() {
        const mode = ref(''); // 'edit', 'delete'
        const category = ref({});
        const message = ref('');
        const isSuccess = ref(true);

        // 獲取查詢參數
        const getQueryParam = (name) => {
            const urlParams = new URLSearchParams(window.location.search);
            const value = urlParams.get(name);
            console.log(`${name}:`, value); // 日誌顯示參數值
            return value;
        };


        // 載入類別數據
        const loadCategory = () => {
            const categoryId = getQueryParam('id');
            if (categoryId) {
                fetch(`/ProductCategories/GetCategory?id=${categoryId}`)
                    .then(response => response.json())
                    .then(data => {
                        if (data.success === false) {
                            message.value = data.message;
                            isSuccess.value = false;
                        } else {
                            category.value = data;
                        }
                    })
                    .catch(error => {
                        console.error('Error loading category:', error);
                        message.value = '無法加載類別數據';
                        isSuccess.value = false;
                    });
            } else {
                message.value = '無效的類別ID';
                isSuccess.value = false;
            }
        };

        // 提交類別數據（編輯）
        const submitCategory = () => {
            const endpoint = mode.value === 'edit'
                ? `/ProductCategories/UpdateCategory`
                : null;

            if (endpoint) {
                console.log('Submitting data:', category.value); // 確保在提交之前數據是正確的

                fetch(endpoint, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(category.value)
                })
                    .then(response => response.json())
                    .then(result => {
                        if (result.success) {
                            // 使用 window.location.replace 來替換當前頁面
                            window.location.href = '/ProductCategories/Index'; // 返回列表頁
                        } else {
                            message.value = result.message;
                            isSuccess.value = false;
                        }
                    })
                    .catch(error => {
                        console.error('Error submitting category:', error);
                        message.value = '提交數據時發生錯誤';
                        isSuccess.value = false;
                    });
            }
        };



        // 刪除類別
        const deleteCategory = () => {
            const confirmation = confirm(`確定刪除服務類別 ${category.value.CategoryName}?`);
            if (confirmation) {
                fetch(`https://localhost:44302/api/CategoriesApi/DeleteCategory`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ categoryId: category.value.Id }) // 確保使用正確的 ID
                })
                    .then(response => response.json())
                    .then(result => {
                        if (result.success) {
                            message.value = result.message;
                            isSuccess.value = true;
                            // 刪除後返回列表頁
                            window.location.href = '/ProductCategories/Index'; // 返回列表頁
                        } else {
                            message.value = result.message;
                            isSuccess.value = false;
                        }
                    })
                    .catch(error => {
                        console.error('刪除類別時發生錯誤:', error);
                        message.value = '刪除類別時發生錯誤';
                        isSuccess.value = false;
                    });
            }
        };


        // 儲存或更新類別
        const saveCategory = () => {
            if (mode.value === 'edit') {
                submitCategory(); // 更新服務類別
            }
        };

        // 返回列表頁
        const goBack = () => {
            window.location.href = '/ProductCategories/Index'; // 返回列表頁
        };


        // On component mount
        onMounted(() => {
            mode.value = getQueryParam('mode'); // 確定模式
            const categoryId = getQueryParam('id');
            console.log('CategoryID from URL:', categoryId); // 檢查是否獲取到了 id
            if (categoryId) {
                loadCategory(); // 獲取類別數據
            }
            if (mode.value === 'edit' || mode.value === 'delete') {
                loadCategory(); // 獲取類別數據
            }
            console.log('Mode:', mode.value);

        });

        return {
            mode,
            category,
            message,
            isSuccess,
            deleteCategory,
            saveCategory,
            goBack
        };
    }
};

const app = createApp(config);
app.mount("#app");