/**************************************************************************************************
Copyright (C) Beijing Beida Software Engineering Development Co., Ltd. & Author
All rights reserved.
Author:Kenshin Cui
Date:3/31/2013 12:11:33 PM
Description:基于Ext4.x的进度追踪组件Progress
Note:
    1.progress数据配置格式：{}

*************************************************************************************************/

Ext.define('BeidaSoft.chart.Progress', {
    extend: 'Ext.panel.Panel', //为避免不兼容旧版本ie，这里直接继承于Panel而不是Ext.draw.Component
    alias: 'widget.bdprogress',
    layout: 'fit',
    viewBox: false,
    border: false,
    drawCmp: null, //绘图画板，私有变量
    //defaultConfig: null,
    progress: null, //进度配置对象
    debug: false, //是否为调试模式
    constructor: function (config) {
        if (config) {
            Ext.apply(this, config);
        }
        this.callParent(arguments);
    },

    initComponent: function () {
        var me = this;
        this.addEvents('mapmouseover', 'mapmouseout', 'mapmousedown', 'mapmouseup'); //定义鼠标移入、移出事件

        //定义此作用域内全局变量
        this.defaultConfig = {//默认进度配置
            width: 800,
            height: 14,
            backgroundColor: { 0: { color: '#EEEEEE' }, 50: { color: '#E5E5E5' }, 80: { color: '#DBDBDB' }, 100: { color: '#EDEDED'} }, //默认背景色
            stageColor: { 0: { color: '#FEEBCC' }, 20: { color: '#FEEFD6' }, 25: { color: '#FEEFD6' }, 50: { color: '#FEE7C2' }, 100: { color: '#FEE7C2'} }, //将要到的阶段颜色
            experiencedColor: { 0: { color: '#F93A3A' }, 20: { color: '#F56868' }, 25: { color: '#F56868' }, 50: { color: '#D60000' }, 100: { color: '#CF0202'} }, //已经经过的颜色
            experiencedInitWidth: 20, //经历初始化长度
            split: {
                width: 1,
                height: 23,
                spliter: {
                    color: '#B3B3B3'
                },
                title: {
                    fill: '#818181',
                    font: 'bold 11pt 仿宋'
                    //,stroke:'#FF0000'
                }
            },
            progressTranlate: {//进度精灵偏移量
                x: 0,
                y: 200
            },
            bubbleSprite: {//气泡精灵最大值
                width: 188
            }
        };

        if (me.progress.defaults) {
            Ext.apply(this.defaultConfig, this.progress.defaults); //用户配置优先
        }

        //绘图对象初始化
        me.drawCmp = Ext.create('Ext.draw.Component', {
            gradients: [
                {
                    id: 'progressBackgroundGradient',
                    angle: 90,
                    stops: me.defaultConfig.backgroundColor
                },
                {
                    id: 'progressExperiencedGradient',
                    angle: 90,
                    stops: me.defaultConfig.experiencedColor
                },
                {
                    id: 'progressStageGradient',
                    angle: 90,
                    stops: me.defaultConfig.stageColor
                }
            ]
        });
        me.items = me.drawCmp;

        this.callParent(arguments);
    },
    //onRender:function(){
    //    this.callParent(arguments);
    //},
    afterRender: function () {//由于各个精灵使用surface添加，而surface必须渲染后才能使用，因此添加精灵操作放到afterRender中
        var me = this;
        this.callParent(arguments);
        //添加进度事件
        this.drawCmp.surface.on('mousedown', function (e, eOpts) {
            var target = e.target;
            var sprite = Ext.get(target.id);
            var mapElementType = target.getAttribute('mapElementType');
            me.fireEvent('mapmousedown', target.getAttribute('mapElementType'), sprite, e, eOpts);
        });
        this.drawCmp.surface.on('mouseup', function (e, eOpts) {
            var target = e.target;
            var sprite = Ext.get(target.id);
            var mapElementType = target.getAttribute('mapElementType');
            me.fireEvent('mapmouseup', target.getAttribute('mapElementType'), sprite, e, eOpts);
        });
        this.drawCmp.surface.on('mouseover', function (e, eOpts) {
            var target = e.target;
            var sprite = Ext.get(target.id);
            var mapElementType = target.getAttribute('mapElementType');
            me.fireEvent('mapmouseover', target.getAttribute('mapElementType'), sprite, e, eOpts);
        });
        this.drawCmp.surface.on('mouseout', function (e, eOpts) {
            var target = e.target;
            var sprite = Ext.get(target.id);
            var mapElementType = target.getAttribute('mapElementType');
            me.fireEvent('mapmouseout', target.getAttribute('mapElementType'), sprite, e, eOpts);
        });

        //程序主要流程
        //me.drawBackground();
        me.drawStage();
        me.drawExperienced();
        //渲染分隔符
        for (var i = 0, len = me.progress.split.length; i < len; i += 1) {
            (function (i) {
                me.drawSplit(me.progress.split[i]);
            } (i));
        }
        //渲染标记
        for (var i = 0, len = me.progress.marker.length; i < len; i += 1) {
            (function (i) {
                me.drawMarker(me.progress.marker[i]);
            } (i));
        }
        me.attachment = { sprite: null, position: me.progress.experienced }; //进度附件，私有属性
        //渲染进度附件
        me.attachment.sprite = me.drawAttachment(me.attachment.position);
    },
    onDestroy: function () {
        delete this.defaultConfig;
        delete this.drawCmp;
        delete this.attachment;
        delete this.bubbleSprite;
    },
    getSpriteCenterCoordinate: function (sprite) {//私有方法，取得区域中心坐标
        var bbox = sprite.getBBox();
        return { x: bbox.x + bbox.width / 2, y: bbox.y + bbox.height / 2 };
    },
    progressMouseover: function (area, toColor, fromColor, spriteComposite, e, t, eOpts) {//私有方法，鼠标移入动画
        var me = this;

    },
    progressMouseout: function (area, toColor, fromColor, spriteComposite, e, t, eOpts) {//私有方法，鼠标移出动画
        var me = this;

    },
    progressMouseup: function (area, toColor, fromColor, spriteComposite, e) {
        var me = this;

    },
    addProgressTip: function (area, sprt) {//添加区域提示，私有方法
        Ext.create('Ext.tip.ToolTip', {
            target: sprt.id,
            trackMouse: true,
            html: area.tip
        });
    },
    //drawBackground: function () {
    //    var me = this;
    //    var bg = Ext.create('Ext.draw.Sprite', {
    //        type: 'rect',
    //        width: me.width || me.defaultConfig.width,
    //        height: me.height || me.defaultConfig.height,
    //        radius: (me.height || me.defaultConfig.height) / 2,
    //        fill: 'url(#progressBackgroundGradient)',
    //        opacity: 0.5,
    //        stroke: '#B3B3B3',
    //        'stroke-width': 1,
    //        y: me.defaultConfig.progressTranlate.y
    //    });
    //    me.drawCmp.surface.add(bg);
    //    bg.show(true);
    //    return bg;
    //},
    //进度背景
    drawStage: function () {
        var me = this;
        var stageOffset = 5;
        var stage = Ext.create('Ext.draw.Sprite', {
            type: 'rect',
            width: me.progress.stage - stageOffset,
            height: me.defaultConfig.height,
            fill: 'url(#progressStageGradient)',
            radius: (me.defaultConfig.height) / 2,
            'stroke-width': 0,
            y: me.defaultConfig.progressTranlate.y
        });
        //设置偏移
        stage.setAttributes({
            translate: {
                x: stageOffset,
                y: 0
            }
        }, true);

        me.drawCmp.surface.add(stage);
        stage.show(true);
        return stage;
    },
    //进度
    drawExperienced: function () {
        var me = this;
        var experienced = Ext.create('Ext.draw.Sprite', {
            type: 'rect',
            width: me.defaultConfig.experiencedInitWidth,
            height: me.defaultConfig.height,
            radius: (me.defaultConfig.height) / 2,
            fill: 'url(#progressExperiencedGradient)',
            'stroke-width': 0,
            y: me.defaultConfig.progressTranlate.y
        });
        me.drawCmp.surface.add(experienced);
        experienced.show(true);

        experienced.animate({
            to: {
                width: me.progress.experienced
            },
            duration: 1500
        });

        return experienced;
    },
    drawSplitTitle: function (split) {
        var me = this;
        var splitTitle = Ext.create('Ext.draw.Sprite', {
            type: 'text',
            text: split.title,
            x: split.position - split.title.length * 3,
            y: me.defaultConfig.progressTranlate.y - me.defaultConfig.height,
            fill: me.defaultConfig.split.title.fill,
            font: me.defaultConfig.split.title.font
            //, stroke: me.defaultConfig.split.title.stroke
        });
        me.drawCmp.surface.add(splitTitle);
        splitTitle.show(true);
    },
    drawSplit: function (split) {//绘制分隔线
        var me = this;
        var spliter = Ext.create('Ext.draw.Sprite', {
            type: 'rect',
            width: me.defaultConfig.split.width,
            height: me.defaultConfig.split.height,
            fill: me.defaultConfig.split.spliter.color,
            'stroke-width': 0,
            x: split.position,
            y: me.defaultConfig.progressTranlate.y + me.defaultConfig.height - me.defaultConfig.split.height
        });
        me.drawCmp.surface.add(spliter);
        spliter.show(true);
        me.drawSplitTitle(split);
    },
    drawMarker: function (mk) {
        var me = this;
        var marker = Ext.create('Ext.draw.Sprite', {
            type: "image",
            src: '/modules/commons/js/ext-4.2/ux/images/arrow_red.png',
            width: 16,
            height: 16,
            x: mk.position,
            y: me.defaultConfig.progressTranlate.y + 16
        });
        //        marker.on('render', function () {
        //            Ext.create('Ext.tip.ToolTip', {
        //                target: marker.id,
        //                trackMouse: true,
        //                html: mk.tip
        //                //,anchor:'top'
        //            });
        //        });
        marker.on('mouseover', function () {
            this.stopAnimation();
            this.setAttributes({
                scale: {
                    x: 1.1,
                    y: 1.1
                }
            }, true);
            if (me.attachment.sprite) {
                var time = Math.abs(this.x - me.attachment.position) * 2000 / 800;
                me.attachment.sprite.stopAnimation();
                me.attachment.sprite.animate({
                    to: {
                        x: (this.x - 78 / 2) + 10
                    },
                    duration: time
                });
            }
            //            if (me.sessionBubble) {
            //                me.sessionBubble.destroy();
            //            }
            //            me.sessionBubble = Ext.create('BeidaSoft.chart.SessionBubble', {
            //                tip: mk.tip,
            //                multiple: 4,
            //                x: this.x,
            //                y: 150
            //            });
            //            me.sessionBubble.init();
            //            for (var i = 0, len = me.sessionBubble.getCount(); i < len; i += 1) {
            //                me.drawCmp.surface.add(me.sessionBubble.getAt(i));
            //            }
            //            me.sessionBubble.show(true);
            //me.drawBubble(mk,this);
            if (me.sessionBubble) {
                me.sessionBubble.destroy();
            }
            me.sessionBubble = Ext.create('BeidaSoft.tip.BubbleTip', {
                id: 'mytip',
                target: marker,
                tip: mk.tip,
                width:230,
                offset: {x:0,y:50}
            });
            me.sessionBubble.showHandler();
        });
        marker.on('mouseout', function () {
            this.stopAnimation();
            this.setAttributes({
                scale: {
                    x: 1,
                    y: 1
                }
            }, true);
        });
        me.drawCmp.surface.add(marker);
        marker.show(true);
        return marker;
    },
    drawTip: function (mk, marker, bubble) {//私有方法，绘制气泡上的提示
        var me = this;
        var spriteTip = Ext.create('Ext.draw.Sprite', {
            type: 'text',
            text: marker.tip,
            width: bubble.width * 3,
            x: bubble.x,
            y: bubble.y,
            fill: me.defaultConfig.split.title.fill,
            font: me.defaultConfig.split.title.font
            //, stroke: me.defaultConfig.split.title.stroke
        });
        me.drawCmp.surface.add(spriteTip);
        spriteTip.show(true);
    },
    drawBubble: function (mk, marker) {//绘制提示气泡，私有方法
        var me = this;
        //显示气泡
        if (me.bubbleSprite) {
            me.bubbleSprite.remove();
        }

        me.bubbleSprite = Ext.create('Ext.draw.Sprite', {
            type: 'path',
            path: 'M60.634,0H1.092C0.489,0,0,0.688,0,1.537V28.81c0,0.848,0.489,1.536,1.092,1.536h3.454l5.287,9.219v-9.219 h50.801c0.604,0,1.092-0.688,1.092-1.536V1.537C61.726,0.688,61.237,0,60.634,0z',
            fill: '#FFA3A3',
            'stroke-width': 1,
            stroke: '#FFFFFF',
            translate: {
                x: marker.x,
                y: 150
            }
        });
        me.drawCmp.surface.add(me.bubbleSprite);
        me.bubbleSprite.show(true);
        var width = me.bubbleSprite.getBBox().width;
        var scaleTime = me.defaultConfig.bubbleSprite.width / width;
        me.bubbleSprite.animate({
            to: {
                scale: {
                    cx: 0,
                    cy: 0,
                    x: scaleTime,
                    y: scaleTime
                },
                translate: {
                    x: marker.x + 30,
                    y: 80
                }
            },
            duration: 200
        });
        me.drawTip(mk, marker, me.bubbleSprite);
    },
    drawAttachment: function (position) {
        var me = this;
        var attachment = Ext.create('Ext.draw.Sprite', {
            type: "image",
            src: '/modules/commons/js/ext-4.2/ux/images/car.png',
            width: 78,
            height: 42,
            x: 0,
            y: me.defaultConfig.progressTranlate.y + 42 - 10
        });
        me.drawCmp.surface.add(attachment);
        attachment.show(true);
        attachment.animate({
            to: {
                x: position - 78 / 2
            },
            duration: 1500
        });
        return attachment;
    },
    drawProgress: function (area, index) {
        var me = this, fromColor = me.getAutoFill(index)[0], toColor = me.getAutoFill(index)[1], groupID = (area.id || area.name);

    }
});