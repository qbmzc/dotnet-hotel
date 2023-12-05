<template>
  <div>
    <!--页面区域-->
    <div class="page-view">
      <div class="table-operations">
        <a-space>
<!--          <a-button type="primary" @click="handleAdd">模拟新增</a-button>-->
          <a-button @click="handleBatchDelete">Delete</a-button>
        </a-space>
      </div>
      <a-table
        size="middle"
        rowKey="id"
        :loading="data.loading"
        :columns="columns"
        :data-source="data.tagList"
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
          <template v-if="column.key === 'status'">
            <a-tag :color="text === '1'? '#2db7f5':'#87d068'">
              {{text === '1'? 'ToBePaid': text === '2'? 'Paid':'Canceled'}}
            </a-tag>
          </template>
          <template v-if="column.key === 'operation'">
            <span>
              <a-popconfirm title="Cancel?" ok-text="yes" cancel-text="No" @confirm="confirmCancel(record)">
                <a>Cancel</a>
              </a-popconfirm>
              <a-divider type="vertical" />
              <a-popconfirm title="Delete?" ok-text="Yes" cancel-text="No" @confirm="confirmDelete(record)">
                <a>Delete</a>
              </a-popconfirm>
            </span>
          </template>
        </template>
      </a-table>
    </div>

  </div>
</template>

<script setup lang="ts">
  import { FormInstance, message } from 'ant-design-vue';
  import { createApi, listApi, updateApi, deleteApi, cancelApi } from '/@/api/order';
  import {getFormatTime} from "/@/utils";


  const columns = reactive([
    {
      title: 'ID',
      dataIndex: 'index',
      key: 'index',
      align: 'center'
    },
    {
      title: 'Username',
      dataIndex: 'username',
      key: 'username',
      align: 'center'
    },
    {
      title: 'Room',
      dataIndex: 'title',
      key: 'title',
      align: 'center',
      customRender: ({text}) => text ? text.substring(0, 10) + '...' : '--'
    },
    {
      title: 'Status',
      dataIndex: 'status',
      key: 'status',
      align: 'center',
      scopedSlots: {customRender: 'status'}
    },
    {
      title: 'OrderTime',
      dataIndex: 'orderTime',
      key: 'orderTime',
      align: 'center',
      customRender: ({text}) => getFormatTime(text, true)
    },
    {
      title: 'Action',
      dataIndex: 'action',
      key: 'operation',
      align: 'center',
      fixed: 'right',
      width: 120,
    },
  ]);

  // 页面数据
  const data = reactive({
    tagList: [],
    loading: false,
    keyword: '',
    selectedRowKeys: [] as any[],
    pageSize: 10,
    page: 1,
  });


  onMounted(() => {
    getDataList();
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
        data.tagList = res.data;
      })
      .catch((err) => {
        data.loading = false;
        console.log(err);
      });
  };


  const rowSelection = ref({
    onChange: (selectedRowKeys: (string | number)[], selectedRows: DataItem[]) => {
      console.log(`selectedRowKeys: ${selectedRowKeys}`, 'selectedRows: ', selectedRows);
      data.selectedRowKeys = selectedRowKeys;
    },
  });

  const confirmCancel = (record: any) => {
    cancelApi({ id: record.id })
        .then((res) => {
          getDataList();
          message.success('The Cancellation Was Successful')
        })
        .catch((err) => {
          message.error(err.msg || 'The Operation Failed');
        });
  };

  const confirmDelete = (record: any) => {
    console.log('delete', record);
    deleteApi({ ids: record.id })
      .then((res) => {
        getDataList();
      })
      .catch((err) => {
        message.error(err.msg || 'The Operation Failed');
      });
  };

  const handleAdd = () => {
    // createApi({
    //   thingId: 1,
    //   userId: 2,
    //   count: 1
    // }).then(res => {
    //   getDataList()
    // }).catch(err => {
    //
    // })
  }

  const handleBatchDelete = () => {
    console.log(data.selectedRowKeys);
    if (data.selectedRowKeys.length <= 0) {
      console.log('hello');
      message.warn('Please Select The Delete Item');
      return;
    }
    deleteApi({ ids: data.selectedRowKeys.join(',') })
      .then((res) => {
        message.success('the deletion is successful');
        data.selectedRowKeys = [];
        getDataList();
      })
      .catch((err) => {
        message.error(err.msg || 'The Operation Failed');
      });
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
