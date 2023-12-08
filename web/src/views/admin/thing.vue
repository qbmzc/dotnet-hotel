<template>
  <div>
    <!--页面区域-->
    <div class="page-view">
      <div class="table-operations">
        <a-space>
          <a-button type="primary" @click="handleAdd">New</a-button>
          <a-button @click="handleBatchDelete">Delete</a-button>
          <a-input-search addon-before="Name" enter-button @search="onSearch" @change="onSearchChange" />
        </a-space>
      </div>
      <a-table
          size="middle"
          rowKey="id"
          :loading="data.loading"
          :columns="columns"
          :data-source="data.dataList"
          :scroll="{ x: 'max-content' }"
          :row-selection="rowSelection"
          :pagination="{
          size: 'default',
          current: data.page,
          pageSize: data.pageSize,
          onChange: (current) => (data.page = current),
          showSizeChanger: false,
          showTotal: (total) => `total: ${total}`,
        }"
      >
        <template #bodyCell="{ text, record, index, column }">
          <template v-if="column.key === 'operation'">
            <span>
              <a @click="handleEdit(record)">Edit</a>
              <a-divider type="vertical" />
              <a-popconfirm title="Is Delete?" ok-text="Y" cancel-text="N" @confirm="confirmDelete(record)">
                <a href="#">Del</a>
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
          width="880px"
          ok-text="Confirm"
          cancel-text="cancel"
          @cancel="handleCancel"
          @ok="handleOk"
      >
        <div>
          <a-form ref="myform" :label-col="{ style: { width: '80px' } }" :model="modal.form" :rules="modal.rules">
            <a-row :gutter="24">
              <a-col span="24">
                <a-form-item label="name" name="title">
                  <a-input placeholder="enter" v-model:value="modal.form.title"></a-input>
                </a-form-item>
              </a-col>
              <a-col span="12">
                <a-form-item label="classification" name="classificationId">
                  <a-select placeholder="select"
                            allowClear
                            :options="modal.cData"
                            :field-names="{ label: 'title', value: 'id',}"
                            v-model:value="modal.form.classificationId">
                  </a-select>
                </a-form-item>
              </a-col>
              <a-col span="12">
                <a-form-item label="Tag">
                  <a-select mode="multiple" placeholder="Select" allowClear v-model:value="modal.form.tags">
                    <template v-for="item in modal.tagData">
                      <a-select-option :value="item.id">{{item.title}}</a-select-option>
                    </template>
                  </a-select>
                </a-form-item>
              </a-col>
              <a-col span="24">
                <a-form-item label="Cover">
                  <a-upload-dragger
                      name="file"
                      accept="image/*"
                      :multiple="false"
                      :before-upload="beforeUpload"
                      v-model:file-list="fileList"
                  >
                    <p class="ant-upload-drag-icon">
                      <template v-if="modal.form.coverUrl">
                        <img :src="'data:image/jpeg;base64,'+modal.form.coverUrl"  style="width: 60px;height: 80px;"/>
                      </template>
                      <template v-else>
                        <file-image-outlined />
                      </template>
                    </p>
                    <p class="ant-upload-text">
                      Please select the cover image you want to upload
                    </p>
                  </a-upload-dragger>
                </a-form-item>
              </a-col>

              <a-col span="24">
                <a-form-item label="Description">
                  <a-textarea placeholder="Description" v-model:value="modal.form.description"></a-textarea>
                </a-form-item>
              </a-col>
              <a-col span="12">
                <a-form-item label="Price" name="price">
                  <a-input-number  placeholder="price" :min="0" v-model:value="modal.form.price" style="width: 100%;"></a-input-number>
                </a-form-item>
              </a-col>
              <a-col span="12">
                <a-form-item label="Window">
                  <a-select placeholder="window" allowClear v-model:value="modal.form.window">
                    <a-select-option key="yes" value="yes">Yes</a-select-option>
                    <a-select-option key="no" value="no">No</a-select-option>
                  </a-select>
                </a-form-item>
              </a-col>
              <a-col span="12">
                <a-form-item label="Facility" >
                  <a-input placeholder="facility" v-model:value="modal.form.sheshi" style="width: 100%;"></a-input>
                </a-form-item>
              </a-col>
              <a-col span="12">
                <a-form-item label="Status" name="status">
                  <a-select placeholder="status" allowClear v-model:value="modal.form.status">
                    <a-select-option key="0" value="0">Shelves</a-select-option>
                    <a-select-option key="1" value="1">TakenOff</a-select-option>
                  </a-select>
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
import { FormInstance, message, SelectProps } from 'ant-design-vue';
import { createApi, listApi, updateApi, deleteApi } from '/@/api/thing';
import {listApi as listClassificationApi} from '/@/api/classification'
import {listApi as listTagApi} from '/@/api/tag'
import {BASE_URL} from "/@/store/constants";
import { FileImageOutlined } from '@ant-design/icons-vue';

const columns = reactive([

  {
    title: 'ID',
    dataIndex: 'index',
    key: 'index',
    width: 60
  },
  {
    title: 'Room',
    dataIndex: 'title',
    key: 'title'
  },
  {
    title: 'Price',
    dataIndex: 'price',
    key: 'price'
  },
  {
    title: 'Window',
    dataIndex: 'window',
    key: 'window'
  },
  {
    title: 'Facility',
    dataIndex: 'sheshi',
    key: 'sheshi'
  },
  {
    title: 'Price',
    dataIndex: 'price',
    key: 'price'
  },
  {
    title: 'Description',
    dataIndex: 'description',
    key: 'description',
    customRender: ({ text, record, index, column }) => text ? text.substring(0, 10) + '...' : '--'
  },
  {
    title: 'Status',
    dataIndex: 'status',
    key: 'status',
    customRender: ({ text, record, index, column }) => text === '0' ? 'Shelves' : 'OffShelf'
  },
  {
    title: 'Action',
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
  modal.form.imageFile = copyFile;
  return false;
};

// 文件列表
const fileList = ref<any[]>([]);

// 页面数据
const data = reactive({
  dataList: [],
  loading: false,
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
  cData: [],
  tagData: [{}],
  form: {
    id: undefined,
    title: undefined,
    classificationId: undefined,
    tags: [],
    repertory: undefined,
    price: undefined,
    window: undefined,
    sheshi: undefined,
    status: undefined,
    cover: undefined,
    coverUrl: undefined,
    imageFile: undefined
  },
  rules: {
    title: [{ required: true, message: 'please enter a name', trigger: 'change' }],
    classificationId: [{ required: true, message: 'please select a category', trigger: 'change' }],
    repertory: [{ required: true, message: 'please enter inventory', trigger: 'change' }],
    price: [{ required: true, message: 'please enter pricing', trigger: 'change' }],
    status: [{ required: true, message: 'please select a status', trigger: 'change' }]
  },
});

const myform = ref<FormInstance>();

onMounted(() => {
  getDataList();
  getCDataList();
  getTagDataList();
});

const getDataList = () => {
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
        data.dataList = res.data;
      })
      .catch((err) => {
        data.loading = false;
        console.log(err);
      });
}

const getCDataList = () => {
  listClassificationApi({}).then(res => {
    modal.cData = res.data
  })
}
const getTagDataList = ()=> {
  listTagApi({}).then(res => {
    res.data.forEach((item, index) => {
      item.index = index + 1
    })
    modal.tagData = res.data
  })
}

const onSearchChange = (e: Event) => {
  data.keyword = e?.target?.value;
  console.log(data.keyword);
};

const onSearch = () => {
  getDataList();
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
  modal.form.cover = undefined
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
    if(record[key]) {
      modal.form[key] = record[key];
    }
  }
//  if(modal.form.cover) {
  //  modal.form.coverUrl = BASE_URL + '/api/staticfiles/image/' + modal.form.cover
    //modal.form.cover = undefined
 // }
   if(modal.form.cover) {
    modal.form.coverUrl = modal.form.cover
    modal.form.cover = undefined
  }
};

const confirmDelete = (record: any) => {
  console.log('delete', record);
  deleteApi({ ids: record.id })
      .then((res) => {
        getDataList();
      })
      .catch((err) => {
        message.error(err.msg || 'the operation failed');
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
        getDataList();
      })
      .catch((err) => {
        message.error(err.msg || 'the operation failed');
      });
};

const handleOk = () => {
  myform.value
      ?.validate()
      .then(() => {
        const formData = new FormData();
        if(modal.editFlag) {
          formData.append('id', modal.form.id)
        }
        formData.append('title', modal.form.title)
        if (modal.form.classificationId) {
          formData.append('classificationId', modal.form.classificationId)
        }
        if (modal.form.tags) {
          modal.form.tags.forEach(function (value) {
            if(value){
              formData.append('tags[]', value)
            }
          })
        }
        if (modal.form.imageFile) {
          formData.append('imageFile', modal.form.imageFile)
        }
        formData.append('description', modal.form.description || '')
        formData.append('price', modal.form.price || '')
        if (modal.form.window) {
          formData.append('window', modal.form.window)
        }
        if (modal.form.sheshi) {
          formData.append('sheshi', modal.form.sheshi)
        }
        if (modal.form.status) {
          formData.append('status', modal.form.status)
        }
        if (modal.editFlag) {
          updateApi(formData)
              .then((res) => {
                hideModal();
                getDataList();
              })
              .catch((err) => {
                console.log(err);
                message.error(err.msg || 'the operation failed');
              });
        } else {
          createApi(formData)
              .then((res) => {
                hideModal();
                getDataList();
              })
              .catch((err) => {
                console.log(err);
                message.error(err.msg || 'the operation failed');
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
  fileList.value = []
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
