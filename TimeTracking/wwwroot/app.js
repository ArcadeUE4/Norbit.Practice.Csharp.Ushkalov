const { createApp } = Vue;

createApp({
    data() {
        return {
            selectedDate: new Date().toISOString().split('T')[0],
            records: [],
            activeTasks: [],
            totalHours: 0,
            stickerClass: 'alert-warning',
            loading: false,
            newRecord: {
                hours: 0,
                description: '',
                workTaskId: 0,
                date: ''
            },
            view: 'records', 
            mode: 'day',    
            allProjects: [],
            newProject: {
                name: '',
                code: '',
                isActive: true
            },
            allTasks: [], 
            newTask: { name: '', projectId: 0, isActive: true },
        };
    },
    computed: {
        isFormValid() {
            return this.newRecord.workTaskId > 0 &&
                this.newRecord.hours > 0 &&
                this.newRecord.description.length > 5;
        },
        stickerMessage() {
            if (this.totalHours < 8) return " — Нужно еще поработать.";
            if (this.totalHours === 8) return " — Идеально выполненный план!";
            return " — Внимание, зафиксирована переработка.";
        }
    },
    watch: {
        view(newVal) {
            if (newVal === 'projects') {
                this.fetchProjects();
                this.fetchAllTasks();
            }
        }
    },
    methods: {
        // Загрузка задач
        async fetchTasks() {
            try {
                const response = await axios.get('/api/tasks');
                this.activeTasks = response.data.filter(t => t.isActive);
            } catch (e) {
                console.error("Не удалось загрузить задачи", e);
            }
        },

        
        async fetchData() {
            if (this.mode === 'day') {
                await this.fetchRecords();
            } else {
                await this.fetchMonthRecords();
            }
        },

        // Загрузка записи под конткретный день
        async fetchRecords() {
            this.loading = true;
            try {
                const response = await axios.get(`/api/timerecords?date=${this.selectedDate}`);
                this.records = response.data.records || [];
                this.totalHours = response.data.totalHours || 0;

                const colorMap = {
                    'Yellow': 'alert-warning',
                    'Green': 'alert-success',
                    'Red': 'alert-danger'
                };
                this.stickerClass = colorMap[response.data.statusColor] || 'alert-secondary';
            } catch (e) {
                console.error("Ошибка при получении данных дня", e);
            } finally {
                this.loading = false;
            }
        },

        // Загрузка записей за месяц
        async fetchMonthRecords() {
            const [year, month] = this.selectedDate.split('-');
            this.loading = true;
            try {
                const res = await axios.get(`/api/timerecords/month/${year}/${month}`);
                this.records = res.data.records || [];
                this.totalHours = res.data.totalMonthHours || 0;
            } catch (e) {
                console.error("Ошибка при получении данных месяца", e);
            } finally {
                this.loading = false;
            }
        },

        // Сохранение записи.
        async saveRecord() {
            this.newRecord.date = this.selectedDate;
            try {
                await axios.post('/api/timerecords', this.newRecord);
                this.newRecord.hours = 0;
                this.newRecord.description = '';
                this.newRecord.workTaskId = 0;
                await this.fetchData();
            } catch (e) {
                alert("Ошибка сохранения: " + (e.response?.data || e.message));
            }
        },

        async fetchAllTasks() {
            try {
                const res = await axios.get('/api/tasks');
                this.allTasks = res.data;
            } catch (e) { console.error(e); }
        },

        async addTask() {
            try {
                // Отправляем на контроллер задач
                await axios.post('/api/tasks', this.newTask);
                this.newTask = { name: '', projectId: 0, isActive: true };
                await this.fetchAllTasks();
                await this.fetchTasks();    
            } catch (e) {
                alert("Ошибка при создании задачи");
            }
        },

        // Учет проектов.
        async fetchProjects() {
            try {
                const res = await axios.get('/api/projects');
                this.allProjects = res.data;
            } catch (e) {
                console.error("Ошибка загрузки проектов", e);
            }
        },

        async addProject() {
            try {
                await axios.post('/api/projects', this.newProject);
                this.newProject = { name: '', code: '', isActive: true };
                await this.fetchProjects();
            } catch (e) {
                alert("Ошибка при создании проекта");
            }
        },

        formatDate(dateStr) {
            if (!dateStr) return "";
            return new Date(dateStr).toLocaleDateString('ru-RU');
        }
    },
    mounted() {
        this.fetchTasks();
        this.fetchData();
    }
}).mount('#app');