const { createApp, ref } = Vue;

const config = {
    setup() {
        const category = ref({
            Id: '',
            Name: '',
            Description: '',
            Image: ''
        });

        const message = ref('');
        const isSuccess = ref(true);

        const saveCategory = () => {
            fetch('/ProductCategories/CreateCategory', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(category.value)
            })
                .then(response => response.json())
                .then(result => {
                    if (result.success) {
                        message.value = result.message;
                        isSuccess.value = true;
                        window.location.href = '/Htmls/ServiceCategory/ServiceCategoryList.html'; // 儲存後返回列表頁
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
        };

        const goBack = () => {
            window.location.href = '/Htmls/ServiceCategory/ServiceCategoryList.html'; // 返回列表頁
        };

        const handleImageUpload = (event) => {
            const file = event.target.files[0];
            if (file) {
                category.value.Image = file.name; // 存儲文件名到 Vue 的數據模型中
            }
        };

        return {
            category,
            message,
            isSuccess,
            saveCategory,
            goBack,
            handleImageUpload
        };
    }
};

const app = createApp(config);
app.mount("#app");
