/**************************************************************************************************
Copyright (C) Beijing Beida Software Engineering Development Co., Ltd. & Author
All rights reserved.
Author:Kenshin Cui
Date:3/26/2013 9:54:39 AM
Description:基于ExtJs 4.x的DataView扩展组件
*************************************************************************************************/
Ext.define('BeidaSoft.view.View', {
    extend: 'Ext.view.View',
    alias: 'widget.bddataview',
    loadMask: {msg:'数据加载中...'},
    initComponent: function () {

        this.callParent(arguments);
    }
});
