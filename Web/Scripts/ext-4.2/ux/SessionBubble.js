/**************************************************************************************************
Copyright (C) Beijing Beida Software Engineering Development Co., Ltd. & Author
All rights reserved.
Author:Kenshin Cui
Date:4/2/2013 1:43:57 PM
Description:基于Ext4.x的会话气泡组件
Note:初始化之后必须首先调用init方法才能使用
*************************************************************************************************/
Ext.define('BeidaSoft.chart.SessionBubble', {
    extend: 'Ext.draw.CompositeSprite',
    alias: 'widget.bdsessionbubble',
    x: 0, //x轴坐标
    y: 0, //y轴坐标
    multiple: 5, //气泡动画中放大倍速
    bubbleSprite: null, //私有属性，气泡精灵
    tipSprite: null, //私有属性，提示精灵
    tip: '', //提示信息
    constructor: function (config) {
        var me = this;
        config = config || {};
        Ext.apply(me, config);
        me.callParent();
    },
    drawBubble: function () {//绘制提示气泡，私有方法
        var bubbleSprite = Ext.create('Ext.draw.Sprite', {
            type: 'path',
            path: 'M60.634,0H1.092C0.489,0,0,0.688,0,1.537V28.81c0,0.848,0.489,1.536,1.092,1.536h3.454l5.287,9.219v-9.219 h50.801c0.604,0,1.092-0.688,1.092-1.536V1.537C61.726,0.688,61.237,0,60.634,0z',
            fill: '#FFA3A3',
            'stroke-width': 1,
            stroke: '#FFFFFF',
            translate: {//注意Path的位置设置必须使用translate,而不像其他精灵一样使用x、y坐标，因为路径本身就是带有坐标的,只能通过偏移设置位置
                x: this.x,
                y: this.y
            }
        });
        return bubbleSprite;
    },
    drawTip: function () {
        var tipSprite = Ext.create('Ext.draw.Sprite', {
            type: 'text',
            text: this.tip,
            font: 'bold 12px 微软雅黑',
            //width: this.width * 3,
            x: this.x,
            y: this.y
        });
        return tipSprite;
    },
    init: function () {
        this.add(this.drawBubble());
        this.add(this.drawTip());
    },
    show: function () {
        var me = this;
        var animateTime = 500;
        var box = this.getAt(0).getBBox();
        //this.callParent(arguments);
        this.getAt(0).show(true);
        this.getAt(0).animate({
            to: {
                scale: {
                    cx: 0,
                    cy: 0,
                    x: this.multiple,
                    y: this.multiple
                },
                translate: {
                    x: this.x + box.width * this.multiple / 5,
                    y: this.y - box.height*this.multiple / 2
                }
            },
            duration: animateTime
        });
        this.getAt(1).setAttributes({
            translate: {
                x: -this.multiple * 8,
                y: -this.multiple * 29
            }
        });
        var task = new Ext.util.DelayedTask(function () {
            if (me.getAt(1)) {
                me.getAt(1).show(true);
            }
        });
        task.delay(animateTime);
    }
});