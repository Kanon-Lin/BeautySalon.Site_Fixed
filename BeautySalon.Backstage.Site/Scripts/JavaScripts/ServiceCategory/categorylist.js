const { createApp, ref } = Vue;

const config = {
    setup() {
        const searchTerm = ref('');
        const categories = ref([]);
        const filteredCategorys = ref([]);

        // ������O���
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
                filteredCategorys.value = data; // ��l�ƹL�o���O
            })
            .catch(error => console.error('Error fetching data:', error));

        const searchCategory = () => {
            // �V��ݵo�e�ШD�H�j�M���O
            fetch(`https://localhost:44302/api/CategoriesApi/SearchCategory?name=${encodeURIComponent(searchTerm.value)}`)
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Network response was not ok');
                    }
                    return response.json();
                })
                .then(data => {
                    filteredCategorys.value = data; // ��s�L�o�᪺���O
                    console.log("Filtered categories:", filteredCategorys.value);
                })
                .catch(error => {
                    console.error('�j�M���O�ɵo�Ϳ��~:', error);
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
