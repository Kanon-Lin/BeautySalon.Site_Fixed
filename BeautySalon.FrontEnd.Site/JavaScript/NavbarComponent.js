const NavbarComponent = {
    name: "NavbarComponent",
    template: `
  <header class="main-header" :class="{ 'navbar-hidden': isNavbarHidden }">
    <nav class="navbar" :class="{ 'expanded': isExpanded }">
      <a @click.prevent="goToHomePage" href="#" class="logo-link">
        <img src="/image/navbar/logo.png" alt="Company Logo" class="logo" />
      </a>
      <div class="nav-links">
        <a v-for="(item, index) in navItems"
           :key="index"
           @click="handleNavigation(item)"
           class="nav-item"
           :class="{ 'disabled': !isAuthenticated && item.requiresAuth }">
          {{ item.text }}
        </a>

        <!-- 下拉 -->
        <div class="nav-item dropdown"
             @mouseenter="showMemberDropdown = true"
             @mouseleave="showMemberDropdown = false">
          <a href="#" class="nav-item" @click="handleMemberAreaClick">
            會員專區
          </a>
         <div v-if="showMemberDropdown" class="dropdown-menu">
    <a v-if="isAuthenticated" @click="navigateTo('MemberInfo.html')" class="dropdown-item">會員資料編輯</a>
    <a v-if="isAuthenticated" @click="navigateTo('Orderlist.html')" class="dropdown-item">我的訂單</a>
    <a v-if="isAuthenticated" @click="navigateTo('AppointmentDetail.html')" class="dropdown-item">我的預約</a>
    <a v-if="!isAuthenticated" @click="loginClick" class="dropdown-item">請先登入</a>
</div>
        </div>
      </div>

      <div class="user-actions">
        <div class="hamburger-menu d-md-none" @click="toggleExpand">
          <span></span>
          <span></span>
          <span></span>
        </div>

        <!--登入登出和註冊 -->
        <div v-if="isAuthenticated" class="user-info" @mouseenter="showLogout = true" @mouseleave="showLogout = false">
          <span v-if="!showLogout" class="user-greeting">您好，{{ user.name }}</span>
          <button v-else @click="logout" class="auth-btn logout-btn">
            <span class="btn-text">登出</span>
            <span class="btn-icon">
              <img src="/image/navbar/logout-icon.png" alt="Logout" class="logout-icon" />
            </span>
          </button>
        </div>
        <div v-else class="auth-buttons">
          <a @click="loginClick" class="auth-btn login-btn">
            <span class="btn-text">登入</span>
            <span class="btn-icon">→</span>
          </a>
          <a @click="registerClick" class="auth-btn register-btn">
            <span class="btn-text">註冊</span>
            <span class="btn-icon">+</span>
          </a>
        </div>
      </div>
    </nav>
  </header>
  `,
    setup() {
        const { ref, onMounted, onUnmounted } = Vue;
        const { user, isAuthenticated, logout: authLogout } = useAuth();

        console.log('isAuthenticated:', isAuthenticated.value);
        console.log('user:', user.value);

        const showLogout = ref(false);
        const showMemberDropdown = ref(false);
        const isNavbarHidden = ref(false);
        const isExpanded = ref(false);

        const navItems = [
            { text: "關於我們", link: "/Html/HomePage.html", requiresAuth: false },
            { text: "服務項目", link: "/Html/Products_.html", requiresAuth: false },
            { text: "店家資訊", link: "/Html/StudioInfo.html", requiresAuth: false },
            { text: "最新消息", link: "/Html/HomePage.html", requiresAuth: false },
            { text: "聯絡我們", link: "/Html/ContactUs.html", requiresAuth: false },
        ];

        const goToHomePage = () => {
            window.location.href = "/Html/HomePage.html";
        };

        const navigateTo = (path) => {
            window.location.href = path;
        };

        const handleNavigation = (item) => {
            if (item.requiresAuth && !isAuthenticated.value) {
                loginClick();
            } else {
                navigateTo(item.link);
            }
        };

        const handleMemberAreaClick = (event) => {
            if (!isAuthenticated.value) {
                event.preventDefault();
                loginClick();
            } else {
                navigateTo('/Html/MemberInfo.html');
            }
        };

        const logout = () => {
            console.log('Logout clicked');
            authLogout();
            console.log('AuthLogout called');
            goToHomePage();
            console.log('Redirecting to home page');
        };

        const loginClick = () => {
            navigateTo("/Html/Login.html");
        };

        const registerClick = () => {
            navigateTo("/Html/Register.html");
        };

        const toggleExpand = () => {
            isExpanded.value = !isExpanded.value;
        };

        let lastScrollPosition = 0;

        const handleScroll = () => {
            const currentScrollPosition = window.pageYOffset;
            if (currentScrollPosition < 0) {
                return;
            }
            isNavbarHidden.value =
                currentScrollPosition > lastScrollPosition &&
                currentScrollPosition > 50;
            lastScrollPosition = currentScrollPosition;

            if (isExpanded.value) {
                isExpanded.value = false;
            }
        };

        onMounted(() => {
            window.addEventListener("scroll", handleScroll);
             console.log('NavbarComponent mounted, isAuthenticated:', isAuthenticated.value)
        });

        onUnmounted(() => {
            window.removeEventListener("scroll", handleScroll);
        });

        return {
            navItems,
            isAuthenticated,
            user,
            showLogout,
            showMemberDropdown,
            isNavbarHidden,
            logout,
            loginClick,
            registerClick,
            handleNavigation,
            handleMemberAreaClick,
            goToHomePage,
            navigateTo,
            isExpanded,
            toggleExpand,
        };
    },
};