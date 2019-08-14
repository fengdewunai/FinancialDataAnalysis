/**************************************************************************************************
Copyright (C) Beijing Beida Software Engineering Development Co., Ltd. & Author
All rights reserved.
Author:Kenshin Cui
Date:3/30/2013 3:53:52 PM
Description:基于Ext4.x的ToolTip扩展组件
*************************************************************************************************/
Ext.define('BeidaSoft.tip.ToolTip', {
    extend: 'Ext.tip.ToolTip',
    alias: 'widget.bdtooltip',
    alternateClassName: 'BeidaSoft.ToolTip',
    xtype: 'bdtooltip',
    initComponent: function () {
        this.callParent(arguments);
    }
});