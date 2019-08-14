/**************************************************************************************************
Copyright (C) Beijing Beida Software Engineering Development Co., Ltd. & Author
All rights reserved.
Author:Kenshin Cui
Date:3/30/2013 3:10:33 PM
Description:基于Ext4.x的GridPanel扩展组件
Note:相比较Ext4.x版本的GridPanel，取消了导出Excel功能,如果需要导出Excel请使用AppGridPanel或者使用Report对应接口
*************************************************************************************************/
Ext.define('BeidaSoft.grid.Panel', {
    extend: 'Ext.grid.Panel',
    alias: 'widget.bdgridpanel',
    alternateClassName: 'BeidaSoft.grid.GridPanel',
    xtype: 'bdgridpanel',
    loadMask: true,
    stripeRows:true,
    autoWidth: true,
    singleSelect: false,
    height: 600,
    frame: true, //注意此属性，如果设置为true，则无法取消border
    idProperty: 'id',
    //columns:[],columns在Ext4.x已经作为标准配置属性，因此此处不需要再配置
    rowNumber: false,
    //    checkBox: false,//在Ext4.x中checkbox使用selType:'checkboxmodel'进行设置
    url: 'url...',
    totalProperty: 'totalProperty',
    root: 'root',
    pageSize: 0,
    pageSizeSelector: '', //页面大小选择器（此参数只有在分页情况下可以使用），取值举例(加入一页有40条数据)：'100,500,all'则会出现40条,100条,500条和全部 供用户选择设置
    beforeBaseParams: null,
    autoExpand: true,
    sortable: true, // 是否支持排序
    autoLoad: false, //是否自动加载
    initComponent: function () {
        var me = this;
        var ld = this.autoLoad;
        this.autoLoad = false; //extjs内部如果接收到autoLoad参数为true会报错，这里转移解决
        var objArrayField = [];
        var objField = null;
        if (me.rowNumber) {
            me.columns.unshift(Ext.create('Ext.grid.RowNumberer'));
        }
        for (var i = 0; i < me.columns.length; ++i) {
            objField = {};
            objField.name = me.columns[i].dataIndex;
            objArrayField.push(objField);
        }
        var sort = true;
        if (!this.sortable) {
            sort = false;
        }
        if (ld) {
            if (typeof (ld) == 'object') {
                for (var p in ld) {
                    this.beforeBaseParams[p] = ld[p];
                    //   ld = { params: this.load };
                }
            }
        }
        //定义Model
        var modelID = 'GridPanelModel' + me.id;
        Ext.define(modelID, {
            extend: 'BeidaSoft.data.Model',
            fields: objArrayField,
            idProperty: me.idProperty
        });
        //定义store
        var st = Ext.create('BeidaSoft.data.Store', {
            autoLoad: ld,
            pageSize: me.pageSize,
            model: modelID,
            proxy: {
                type: 'ajax',
                url: me.url,
                timeout: 9999999,
                reader: {
                    root: me.root,
                    totalProperty: me.totalProperty
                }
                //                ,simpleSortMode: true
            }
        });

        me.store = st;
        if (!me.viewConfig) {
            me.viewConfig = {
                trackOver: false,
                stripeRows: me.stripeRows,
                forceFit: true
            };
        }

        if (me.beforeBaseParams) {
            st.on("beforeload", function (store, options) {
                Ext.apply(me.store.getProxy().extraParams, me.beforeBaseParams);
            })
        }

        if (me.pageSize != 0) {
            me.bbar = me.getPagingToolbar();
        }

        this.callParent(arguments);
    },
    loadPage: function (page) {//按页数加载数据
        this.store.loadPage(page);
    },
    load: function (para) {
        //        var paraObj = { params: {} };
        if (para) {
            Ext.apply(this.store.getProxy().extraParams, para);
        }
        //        if (this.pageSize > 0) {
        //            paraObj.params.start = 0;
        //            paraObj.params.limit = this.pageSize;
        //        }
        //this.store.load(paraObj);此方法仍然支持，但是不建议
        this.loadPage(1);
    },
    getPagingToolbar: function () {//取得分页栏，私有方法
        var me = this;
        var pgtb;
        if (me.pageSizeSelector != '') {//如果现实全部显示
            var pageSizeArray = [];
            var selectorArray = me.pageSizeSelector.split(',');
            for (var i = 0, len = selectorArray.length; i < len; ++i) {
                if (i == 0 && selectorArray[0] != me.pageSize) {
                    pageSizeArray.push([Number(me.pageSize), me.pageSize + '条']);
                }
                if (selectorArray[i].toUpperCase() == 'ALL') {
                    pageSizeArray.push([999999999, '全部']);
                } else {
                    pageSizeArray.push([selectorArray[i], selectorArray[i] + '条']);
                }
            }
            var pageSizeStore = new Ext.data.SimpleStore({
                fields: ['pageSize', 'pageSizeDesc'],
                data: pageSizeArray
            });
            var cbPageSize = new Ext.form.ComboBox({
                store: pageSizeStore,
                displayField: 'pageSizeDesc',
                valueField: 'pageSize',
                typeAhead: true,
                mode: 'local',
                triggerAction: 'all',
                emptyText: '选择每页显示记录数',
                selectOnFocus: true,
                value: me.pageSize + '条',
                width: 80,
                listeners: {
                    'select': function (combobox, record, index) {
                        var newPageSize = record.data['pageSize'];
                        if (me.pageSize != newPageSize) {
                            me.pageSize = Number(newPageSize);
                            pgtb.pageSize = Number(newPageSize);
                            //var startParams = { params: { start: 0, limit: Number(newPageSize)} };
                            var lastParams = me.store.lastOptions;
                            //Ext.applyIf(startParams, lastParams);
                            me.load(lastParams.params);
                        }
                    }
                }
            });
            pgtb = Ext.create('Ext.PagingToolbar', {
                displayInfo: false,
                store: me.store,
                pageSize: me.pageSize,
                items: ['->', '每页显示', cbPageSize]
            });
        } else {
            pgtb = Ext.create('Ext.PagingToolbar', {
                displayInfo: true,
                emptyMsg: "没有数据要显示！",
                displayMsg: "当前为第{0}--{1}条，共{2}条数据",
                store: me.store,
                pageSize: me.pageSize
            });
        }
        return pgtb;
    },
    getSelectedRows: function (asc) {
        //        var ascOrder = true;
        //        if (arguments.length == 1) {
        //            ascOrder = asc;
        //        }
        //        var compare = function (propertyName) {
        //            return function (object1, object2) {
        //                var value1 = object1[propertyName];
        //                var value2 = object2[propertyName];
        //                if (ascOrder) {
        //                    return value1 - value2;
        //                } else {
        //                    return value2 - value1;
        //                }
        //            }
        //        }
        var records = this.getSelectionModel().getSelection(); //ExtJs4.x不再支持getSelections()统一为getSelection()或者selected属性(selected.items)
        //        var ar = [];
        //        Ext.each(records, function (record) {
        //            record.rowIndex = this.getStore().indexOf(record);
        //            ar.push(record);
        //        }, this);
        //        ar.sort(compare('rowIndex'));
        //        return ar;
        return records;
    },
    getAllRows: function (asc) {
        var ascOrder = true;
        if (arguments.length == 1) {
            ascOrder = asc;
        }
        var compare = function (propertyName) {
            return function (object1, object2) {
                var value1 = object1[propertyName];
                var value2 = object2[propertyName];
                if (ascOrder) {
                    return value1 - value2;
                } else {
                    return value2 - value1;
                }
            }
        }
        var records = this.getStore().data.items;
        var ar = [];
        Ext.each(records, function (record) {
            record.rowIndex = this.getStore().indexOf(record);
            ar.push(record);
        }, this);
        ar.sort(compare('rowIndex'));
        return ar;
    },
    getSelectedSingleRow: function () {
        var records = this.getSelectedRows();
        if (records) {
            return records[0];
        } else {
            return null;
        }
    },
    getSelectedSingleRowField: function (fieldName) {
        var record = this.getSelectionModel().getSelected();
        if (record) {
            return record.get(fieldName);
        } else {
            return null;
        }
    },
    moveUp: function () {
        var selectedRows = this.getSelectedRows();
        if (selectedRows.length == 0) {
            console.log('请选择数据后再进行排序');
            return;
        }

        if (this.getSelectionModel().isSelected(0)) {
            console.log('当前数据已经处于最顶部！');
            return;
        }
        var currentRowIndex = 0;
        for (var i = 0; i < selectedRows.length; ++i) {
            currentRowIndex = this.store.indexOf(selectedRows[i]);
            this.store.data.items[currentRowIndex] = this.store.data.items[currentRowIndex - 1];
            this.store.data.items[currentRowIndex - 1] = selectedRows[i];
        }
        this.getSelectionModel().deselectAll();
        this.getView().refresh();
        this.getSelectionModel().select(selectedRows, true, true); //注意最后一个参数，如果为false容易内存溢出
    },
    moveDown: function () {
        var selectedRows = this.getSelectedRows();
        if (selectedRows.length == 0) {
            console.log('请选择数据后再进行排序');
            return;
        }
        if (this.getSelectionModel().isSelected((this.store.getCount() - 1))) {
            BeidaSoft.Msg.info('当前数据已经处于最底部！');
            return;
        }
        var currentRowIndex = 0;
        for (var i = selectedRows.length - 1; i >= 0; --i) {
            currentRowIndex = this.store.indexOf(selectedRows[i]); // selectedRows[i].rowIndex;
            this.store.data.items[currentRowIndex] = this.store.data.items[currentRowIndex + 1];
            this.store.data.items[currentRowIndex + 1] = selectedRows[i];
        }
        this.getSelectionModel().deselectAll();
        this.getView().refresh();
        this.getSelectionModel().select(selectedRows, true, true); //Ext4.x不再支持selectRecords
    },
    selectRows: function (field, values) {//field：选择时依据的列名，values：对应列的值，多个值之间用“,”分割
        var selectRows = [];
        var valueArray = values.split(',');
        for (var i = 0; i < valueArray.length; ++i) {
            selectRows.push(this.store.getAt(this.store.find(field, valueArray[i])));
        }
        //        this.getSelectionModel().clearSelections();
        //        this.getView().refresh();
        this.getSelectionModel().selectRecords(selectRows);
    }
}); 