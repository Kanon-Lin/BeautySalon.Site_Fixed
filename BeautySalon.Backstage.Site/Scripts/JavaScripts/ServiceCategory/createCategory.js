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
                        window.location.href = '/Htmls/ServiceCategory/ServiceCategoryList.html'; // �x�s���^�C��
                    } else {
                        message.value = result.message;
                        isSuccess.value = false;
                    }
                })
                .catch(error => {
                    console.error('Error submitting category:', error);
                    message.value = '����ƾڮɵo�Ϳ��~';
                    isSuccess.value = false;
                });
        };

        const goBack = () => {
            window.location.href = '/Htmls/ServiceCategory/ServiceCategoryList.html'; // ��^�C��
        };

        const handleImageUpload = (event) => {
            const file = event.target.files[0];
            if (file) {
                category.value.Image = file.name; // �s�x���W�� Vue ���ƾڼҫ���
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
