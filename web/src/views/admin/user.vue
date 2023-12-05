<template>
  <div>
    <!--页面区域-->
    <div class="page-view">
      <div class="table-operations">
        <a-space>
          <a-button type="primary" @click="handleAdd">New</a-button>
          <a-button @click="handleBatchDelete">Delete</a-button>
          <a-input-search addon-before="Username" enter-button @search="onSearch" @change="onSearchChange" />
        </a-space>
      </div>
      <a-table
        size="middle"
        rowKey="id"
        :loading="data.loading"
        :columns="columns"
        :data-source="data.userList"
        :scroll="{ x: 'max-content' }"
        :row-selection="rowSelection"
        :pagination="{
          size: 'default',
          current: data.page,
          pageSize: data.pageSize,
          onChange: (current) => (data.page = current),
          showSizeChanger: false,
          showTotal: (total) => `Total ${total}`,
        }"
      >
        <template #bodyCell="{ text, record, index, column }">
          <template v-if="column.key === 'role'">
            <span>
              <span v-if="text === '1'">RegularUsers</span>
              <!-- <span v-if="text === '2'">DemoAccount</span> -->
              <span v-if="text === '3'">Administrator</span>
            </span>
          </template>
          <template v-if="column.key === 'operation'">
            <span>
              <a @click="handleEdit(record)">Edit</a>
              <a-divider type="vertical" />
              <a-popconfirm title="Delete?" ok-text="Yes" cancel-text="No" @confirm="confirmDelete(record)">
                <a href="#">Delete</a>
              </a-popconfirm>
            </span>
          </template>
        </template>
      </a-table>
    </div>

    <!--弹窗区域-->
    <div>
      <a-modal
        :visible="modal.visile"
        :forceRender="true"
        :title="modal.title"
        ok-text="Yes"
        cancel-text="No"
        @cancel="handleCancel"
        @ok="handleOk"
      >
        <div>
          <a-form ref="myform" :label-col="{ style: { width: '80px' } }" :model="modal.form" :rules="modal.rules">
            <a-row :gutter="24">
              <a-col span="24">
                <a-form-item label="username" name="username">
                  <a-input :disabled="modal.editFlag" placeholder="username" v-model:value="modal.form.username" allowClear />
                </a-form-item>
              </a-col>
              <a-col span="24" v-if="!modal.editFlag">
                <a-form-item label="password" name="password">
                  <a-input placeholder="password" type="password" v-model:value="modal.form.password" allowClear />
                </a-form-item>
              </a-col>
              <a-col span="24">
                <a-form-item label="nickname" name="nickname">
                  <a-input placeholder="nickname" v-model:value="modal.form.nickname" allowClear />
                </a-form-item>
              </a-col>
              <a-col span="24">
                <a-form-item label="role" name="role">
                  <a-select placeholder="role" allowClear v-model:value="modal.form.role">
                    <template v-for="item in modal.roleData">
                      <a-select-option :value="item.id">{{ item.title }}</a-select-option>
                    </template>
                  </a-select>
                </a-form-item>
              </a-col>
              <a-col span="24">
                <a-form-item label="status" name="status">
                  <a-select placeholder="status" allowClear v-model:value="modal.form.status">
                    <a-select-option key="0" value="0">Active</a-select-option>
                    <a-select-option key="1" value="1">Inactive</a-select-option>
                  </a-select>
                </a-form-item>
              </a-col>
              <a-col span="24">
                <a-form-item label="email" name="email">
                  <a-input placeholder="email" v-model:value="modal.form.email" allowClear />
                </a-form-item>
              </a-col>
              <a-col span="24">
                <a-form-item label="mobile" name="mobile">
                  <a-input placeholder="mobile" v-model:value="modal.form.mobile" allowClear />
                </a-form-item>
              </a-col>
            </a-row>
          </a-form>
        </div>
      </a-modal>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { FormInstance, message } from 'ant-design-vue';
  import { createApi, listApi, updateApi, deleteApi } from '/@/api/user';
  import {getFormatTime} from "/@/utils";

  const MOCK_USERS = [
    {
      index: '1',
      username: 'admin123',
      role: 'Administrator',
      email: '23222@qq.com',
      remark: 'Remark 11112233',
    },
    {
      index: '1',
      username: 'admin123',
      role: 'Administrator',
      email: '23222@qq.com',
      remark: '我是备注 我是备注11112233',
    },
    {
      index: '1',
      username: 'admin123',
      role: 'Administrator',
      email: '23222@qq.com',
      remark: '我是备注 我是备注11112233',
    },
  ];

  const columns = reactive([
    {
      title: 'ID',
      dataIndex: 'index',
      key: 'index',
      align: 'center',
    },
    {
      title: 'username',
      dataIndex: 'username',
      key: 'username',
      align: 'center',
    },
    {
      title: 'nickname',
      dataIndex: 'nickname',
      key: 'nickname',
      align: 'center',
    },
    {
      title: 'role',
      dataIndex: 'role',
      key: 'role',
      align: 'center',
    },
    {
      title: 'status',
      dataIndex: 'status',
      key: 'status',
      align: 'center',
      customRender: ({ text, record, index, column }) => (text === '0' ? '正常' : '封号'),
    },
    {
      title: 'email',
      dataIndex: 'email',
      key: 'email',
      align: 'center',
    },
    {
      title: 'mobile',
      dataIndex: 'mobile',
      key: 'mobile',
      align: 'center',
    },
    {
      title: 'createTime',
      dataIndex: 'createTime',
      key: 'createTime',
      align: 'center',
      customRender: ({text}) => getFormatTime(text, true)
    },
    {
      title: 'action',
      dataIndex: 'action',
      key: 'operation',
      align: 'center',
      fixed: 'right',
      width: 140,
    },
  ]);

  const beforeUpload = (file: File) => {
    // 改文件名
    const fileName = new Date().getTime().toString() + '.' + file.type.substring(6);
    const copyFile = new File([file], fileName);
    console.log(copyFile);
    modal.form.cover = copyFile;
    return false;
  };

  const fileList = ref([]);

  // 页面数据
  const data = reactive({
    userList: [],
    loading: false,
    currentAdminUserName: '',
    keyword: '',
    selectedRowKeys: [] as any[],
    pageSize: 10,
    page: 1,
  });

  // 弹窗数据源
  const modal = reactive({
    visile: false,
    editFlag: false,
    title: '',
    roleData: [
      {
        id: '1',
        title: 'RegularUsers',
      },
      {
        id: '2',
        title: 'DemoAccount',
      },
      {
        id: '3',
        title: 'Administrator',
      },
    ],
    form: {
      id: undefined,
      username: undefined,
      password: undefined,
      role: undefined,
      status: undefined,
      nickname: undefined,
      email: undefined,
      mobile: undefined,
    },
    rules: {
      username: [{ required: true, message: 'please enter', trigger: 'change' }],
      password: [{ required: true, message: 'please enter', trigger: 'change' }],
      role: [{ required: true, message: 'please select', trigger: 'change' }],
      status: [{ required: true, message: 'please select', trigger: 'change' }],
    },
  });

  const myform = ref<FormInstance>();

  onMounted(() => {
    getUserList();
  });

  const getUserList = () => {
    data.loading = true;
    listApi({
      keyword: data.keyword,
    })
      .then((res) => {
        data.loading = false;
        console.log(res);
        res.data.forEach((item: any, index: any) => {
          item.index = index + 1;
        });
        data.userList = res.data;
      })
      .catch((err) => {
        data.loading = false;
        console.log(err);
      });
  };

  const onSearchChange = (e: Event) => {
    data.keyword = e?.target?.value;
    console.log(data.keyword);
  };

  const onSearch = () => {
    getUserList();
  };

  const rowSelection = ref({
    onChange: (selectedRowKeys: (string | number)[], selectedRows: DataItem[]) => {
      console.log(`selectedRowKeys: ${selectedRowKeys}`, 'selectedRows: ', selectedRows);
      data.selectedRowKeys = selectedRowKeys;
    },
  });

  const handleAdd = () => {
    resetModal();
    modal.visile = true;
    modal.editFlag = false;
    modal.title = 'New';
    // 重置
    for (const key in modal.form) {
      modal.form[key] = undefined;
    }
  };
  const handleEdit = (record: any) => {
    resetModal();
    modal.visile = true;
    modal.editFlag = true;
    modal.title = 'Edit';
    // 重置
    for (const key in modal.form) {
      modal.form[key] = undefined;
    }
    for (const key in record) {
      modal.form[key] = record[key];
    }
  };

  const confirmDelete = (record: any) => {
    console.log('delete', record);
    deleteApi({ ids: record.id })
      .then((res) => {
        getUserList();
      })
      .catch((err) => {
        message.warn(err.msg || "the operation failed")
      });
  };

  const handleBatchDelete = () => {
    console.log(data.selectedRowKeys);
    if (data.selectedRowKeys.length <= 0) {
      console.log('hello');
      message.warn('please select the delete item');
      return;
    }
    deleteApi({ ids: data.selectedRowKeys.join(',') })
      .then((res) => {
        message.success('the deletion is successful');
        data.selectedRowKeys = [];
        getUserList();
      })
      .catch((err) => {
        message.warn(err.msg || "the operation failed")
      });
  };

  const handleOk = () => {
    myform.value
      ?.validate()
      .then(() => {
        const formData = new FormData();
        if (modal.form.id) {
          formData.append('id', modal.form.id);
        }
        if (modal.form.username) {
          formData.append('username', modal.form.username);
        }
        if (modal.form.password) {
          formData.append('password', modal.form.password);
        }
        if (modal.form.nickname) {
          formData.append('nickname', modal.form.nickname);
        }
        if (modal.form.role) {
          formData.append('role', modal.form.role);
        }
        if (modal.form.status) {
          formData.append('status', modal.form.status);
        }
        if (modal.form.cover) {
          formData.append('cover', modal.form.cover);
        }
        if (modal.form.mobile) {
          formData.append('mobile', modal.form.mobile);
        }
        if (modal.form.email) {
          formData.append('email', modal.form.email);
        }
        if (modal.editFlag) {
          updateApi(formData)
            .then((res) => {
              hideModal();
              getUserList();
            })
            .catch((err) => {
              console.log(err);
              message.warn(err.msg || "the operation failed")
            });
        } else {
          createApi(formData)
            .then((res) => {
              hideModal();
              getUserList();
            })
            .catch((err) => {
              console.log(err);
              message.warn(err.msg || "the operation failed")
            });
        }
      })
      .catch((err) => {
        console.log('it can t be empty');
      });
  };

  const handleCancel = () => {
    hideModal();
  };

  // 恢复表单初始状态
  const resetModal = () => {
    myform.value?.resetFields();
  };

  // 关闭弹窗
  const hideModal = () => {
    modal.visile = false;
  };
</script>

<style scoped lang="less">
  .page-view {
    min-height: 100%;
    background: #fff;
    padding: 24px;
    display: flex;
    flex-direction: column;
  }

  .table-operations {
    margin-bottom: 16px;
    text-align: right;
  }

  .table-operations > button {
    margin-right: 8px;
  }
</style>
