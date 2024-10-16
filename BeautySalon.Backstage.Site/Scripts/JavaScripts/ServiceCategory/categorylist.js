const { createApp, ref } = Vue;

const config = {
    setup() {
        const searchTerm = ref('');
        const categories = ref([]);
        const filteredCategorys = ref([]);

        // 獲取類別資料
        fetch('/ProductCategories/CategoryIndex')
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(data => {
                console.log('Fetched data:', data);
                categories.value = data;
                filteredCategorys.value = data; // 初始化過濾類別
            })
            .catch(error => console.error('Error fetching data:', error));

        const searchCategory = () => {
            // 向後端發送請求以搜尋類別
            fetch(`https://localhost:44302/api/CategoriesApi/SearchCategory?name=${encodeURIComponent(searchTerm.value)}`)
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Network response was not ok');
                    }
                    return response.json();
                })
                .then(data => {
                    filteredCategorys.value = data; // 更新過濾後的類別
                    console.log("Filtered categories:", filteredCategorys.value);
                })
                .catch(error => {
                    console.error('搜尋類別時發生錯誤:', error);
                });
        };

        const addCategory = () => {
            window.location.href = `CreateServiceCategory.html?mode=create`;
        };

        const editCategory = (Id) => {
            const url = `EditServiceCategory.html?id=${Id}&mode=edit`;
            console.log('Generated URL:', url);
            window.location.href = url;
        };

        const deleteCategory = (Id) => {
            window.location.href = `DeleteServiceCategory.html?id=${Id}&mode=delete`;
        };

        return {
            searchTerm,
            categories,
            filteredCategorys,
            searchCategory,
            addCategory,
            editCategory,
            deleteCategory
        };
    }
};

const app = createApp(config);
app.mount('#app');
