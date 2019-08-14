/**************************************************************************************************
Copyright (C) Beijing Beida Software Engineering Development Co., Ltd. & Author
All rights reserved.
Author:Kenshin Cui
Date:3/26/2013 10:11:24 AM
Description:组合控件，对View扩展组件，提供了类似分页等扩展功能
*************************************************************************************************/
Ext.define('BeidaSoft.app.ViewPanel', {
    extend: 'BeidaSoft.panel.Panel',
    alias: 'widget.bdaviewpanel',
    layout: 'fit',
    dataView: null, //私有属性
    tpl: null, //视图模板，如果不设置将使用内置模板
    itemSelector: 'div.viewitem', //此属性必须设置否则在低版本IE中报错
    overItemCls: 'viewitem-hover', //此属性ext4.x做了变更不再叫overClass
    singleSelect: true,
    multiSelect: true,
    loadMask: { msg: '数据加载中...' },
    //autoScroll: false, //panel的autoScroll设置为false、其组件dataview的autoscroll设置为true，否则滚动后会造成loadMask向上下移动
    columnCount: 0, //要显示的列数，默认为0，所有列均显示
    url: 'url...', //此url只是sql文件处理路径，同BeidaSoft.grid.GridPanel意义不同并非一个请求
    beforeBaseParams: null,
    //sortable: false, // 是否支持排序,这里指的是列排序
    //sortConfig: { tableName: '', primaryKey: '', sortField: '', primaryKeyValues: '' }, //primaryKeyValues是组件内部参数不对外公开
    cacheData: false, //是否对数据进行缓存
    autoLoad: false, //是否自动加载
    tip: false, //启用提示,如果启用可以配置{id:'A00',tpl:''}
    debug: false, //是否开启调试模式,调试模式（并且不缓存）的情况下可以查看后台sql
    initComponent: function () {
        var me = this;
        this.autoScroll = false;
        this.dataView = Ext.create('BeidaSoft.app.View', {
            id: me.id,
            tpl: me.tpl,
            itemSelector: me.itemSelector,
            overItemCls: me.overItemCls,
            singleSelect: me.singleSelect,
            multiSelect: me.multiSelect,
            loadMask: me.loadMask,
            autoScroll: true,
            columnCount: me.columnCount,
            url: me.url,
            beforeBaseParams: me.beforeBaseParams,
            cacheData: me.cacheData,
            autoLoad: me.autoLoad,
            tip:me.tip,
            debug: me.debug
        });
        this.id = this.id + '_Panel';
        this.items = this.dataView;
        if (this.pageSize != 0) {
            this.bbar = this.getPagingToolbar();
        }

        this.callParent(arguments);
    },
    onRender: function () {
        this.callParent(arguments);
    },
    onDestroy: function () {
        this.callParent(arguments);
        delete this.dataView;
    },
    getPagingToolbar: function () {
        var pb = new Ext.PagingToolbar({
            displayInfo: true,
            emptyMsg: "没有数据要显示！",
            displayMsg: "当前为第{0}--{1}条，共{2}条数据",
            pageSize: this.pageSize,
            store: this.dataView.getStore()
        });
        return pb;
    },
    load: function (para) {
        this.dataView.load(para);
    },
    getStore: function () {
        return this.dataView.getStore();
    },
    getRecord: function () {
        return this.dataView.getRecord();
    }
});