const { createApp, ref, onMounted } = Vue;

const config = {
    setup() {
        const mode = ref(''); // 'create', 'edit', 'view', 'delete'
        const service = ref({
            ProductId: '',         // 服務項目ID
            ProductName: '',       // 服務名稱
            CategoryId: '',        // 服務類別ID
            Price: '',             // 價格
            Duration: '',          // 施作時間
            CreatedDate: '',       // 建檔日期
            Description: '',       // 服務描述
        });
        const categories = ref([]); // 服務類別

        // 獲取查詢參數
        const getQueryParam = (name) => {
            const urlParams = new URLSearchParams(window.location.search);
            const value = urlParams.get(name);
            console.log(`查詢參數 ${name}: ${value}`); // 確認查詢參數
            return value;
        };


        // 載入服務類別
        const loadCategories = () => {
            console.log('開始載入服務類別'); // 這行用來確認函式開始執行
            fetch('https://localhost:44302/api/ProductsApi/GetCategory')
                .then(response => {
                    console.log('收到回應'); // 確認有收到回應
                    if (!response.ok) {
                        throw new Error('Network response was not ok');
                    }
                    return response.json();
                })
                .then(data => {
                    console.log('收到資料:', data); // 查看返回的資料
                    categories.value = data;
                    console.log('更新後的類別:', categories.value); // 檢查載入的類別
                })
                .catch(error => {
                    console.error('無法獲取服務類別:', error);
                });
        };


        // 載入服務項目
        const loadService = () => {
            const serviceId = getQueryParam('id');
            console.log(`查詢參數 ID: ${serviceId}`);
            if (serviceId) {
                fetch(`https://localhost:44302/api/ProductsApi/GetProduct/${serviceId}`)
                    .then(response => {
                        if (!response.ok) {
                            throw new Error('Network response was not ok');
                        }
                        return response.json();
                    })
                    .then(existingService => {
                        service.value = { ...existingService };
                        const category = categories.value.find(cat => cat.CategoryId === service.value.CategoryId);
                        if (category) {
                            service.value.CategoryName = category.CategoryName;
                        }
                    })
                    .catch(error => {
                        console.error('無法獲取服務項目:', error);
                    });
            }
        };

        const deleteService = () => {
            const confirmation = confirm(`確定刪除服務項目 ${service.value.ProductName}?`);
            if (confirmation) {
                const url = `https://localhost:44302/api/ProductsApi/DeleteProduct/${service.value.ProductId}`;  // 使用正確的 API URL

                fetch(url, {
                    method: 'DELETE', // 使用 DELETE 方法
                    headers: {
                        'Content-Type': 'application/json',
                    },
                })
                    .then(response => {
                        if (!response.ok) {
                            throw new Error('Network response was not ok');
                        }
                        alert(`服務項目 ${service.value.ProductName} 已成功刪除`);
                        window.location.href = '/ProductCategories/ProductIndex'; // 返回列表頁
                    })
                    .catch(error => {
                        console.error('刪除失敗:', error);
                        alert('刪除失敗');
                    });
            }
        };


        const saveService = () => {
            console.log('当前选中的服务类别 ID:', service.value.CategoryId);

            // 檢查 Duration 是否為 30 的倍數
            if (service.value.Duration % 30 !== 0) {
                alert('施作時長必須是 30 分鐘的倍數');
                return; // 停止執行
            }

            // 如果有 ProductId，則為更新，否則為新增
            const isUpdate = !!service.value.ProductId;
            const url = isUpdate
                ? `https://localhost:44302/api/ProductsApi/UpdateProduct/${service.value.ProductId}`
                : 'https://localhost:44302/api/ProductsApi/CreateProduct';

            const requestBody = {
                //ProductId: service.value.ProductId,
                ProductName: service.value.ProductName,
                CategoryId: service.value.CategoryId,
                Price: service.value.Price,
                Duration: service.value.Duration,
                CreatedDate: service.value.CreatedDate.toString(),
                Description: service.value.Description,
            };

            console.log("requestBody:", requestBody);

            fetch(url, {
                method: isUpdate ? 'PUT' : 'POST',  // 更新使用 PUT，新增使用 POST
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(requestBody),
            })
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Network response was not ok');
                    }
                    alert(isUpdate
                        ? `服務項目已更新: ${requestBody.ProductName}`
                        : `服務項目已新增: ${requestBody.ProductName}`);
                    window.location.href = '/ProductCategories/ProductIndex'; // 返回列表頁

                })
                .catch(error => {
                    console.error('無法儲存服務項目:', error);
                });
        };




        const goBack = () => {
            window.location.href = '/ProductCategories/ProductIndex'; // 返回列表頁

        };

        // 格式化日期和時間為24小時制
        const formatDate = (dateString) => {
            const date = new Date(dateString);
            const year = date.getFullYear();
            const month = String(date.getMonth() + 1).padStart(2, '0');
            const day = String(date.getDate()).padStart(2, '0');
            const hours = String(date.getHours()).padStart(2, '0');  // 24小時制
            const minutes = String(date.getMinutes()).padStart(2, '0');
            return `${year}-${month}-${day} ${hours}:${minutes}`;  // 確保 datetime-local 格式符合24小時制
        };

        // 在組件掛載時確定模式並載入資料
        onMounted(() => {
            console.log('Vue 應用已掛載'); // 確認掛載
            mode.value = getQueryParam('mode');
            if (mode.value === 'edit' || mode.value === 'delete') {
                loadCategories();
                loadService();
            } else if (mode.value === 'create') {
                loadCategories();
            }
        });


        return {
            mode,
            service,
            categories,
            deleteService,
            saveService,
            goBack,
            loadCategories,
            loadService,
            formatDate
        };
    }
};

const app = createApp(config);
app.mount('#app');
